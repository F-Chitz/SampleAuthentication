// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleAuthentication.Areas.Identity.Data;

namespace SampleAuthentication.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<SampleAuthenticationUser> _userManager;
        private readonly SignInManager<SampleAuthenticationUser> _signInManager;

        public IndexModel(
            UserManager<SampleAuthenticationUser> userManager,
            SignInManager<SampleAuthenticationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Preferred Name")]
            public string PreferredName { get; set; }

            public bool HasPageI { get; set; }
            public bool HasPageII { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(SampleAuthenticationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            bool _HasPageI = false;
            bool _HasPageII = false;
            var claims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in claims)
            {
                switch (claim.Type)
                {
                    case "HasPageI":
                        _HasPageI = true;
                        break;
                    case "HasPageII":
                        _HasPageII = true;
                        break;

                }
            }

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = user.FullName,
                PreferredName = user.PreferredName,
                HasPageI = _HasPageI,
                HasPageII = _HasPageII
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.FullName = Input.FullName;
            user.PreferredName = Input.PreferredName;

            await ManageClaimsAsync(user);

            await _userManager.UpdateAsync(user);


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }


        private async Task ManageClaimsAsync(SampleAuthenticationUser user)
        {
            var usersClaims = await _userManager.GetClaimsAsync(user);


            if (Input.HasPageI && !usersClaims.Any(x => x.Type == "HasPageI"))
            {
                await _userManager.AddClaimAsync(user, new Claim("HasPageI", Input.HasPageI.ToString()));
            }
            else if (!Input.HasPageI && usersClaims.Any(x => x.Type == "HasPageI"))
            {
                await _userManager.RemoveClaimAsync(user, usersClaims.FirstOrDefault(x => x.Type == "HasPageI"));
            }

            if (Input.HasPageII && !usersClaims.Any(x => x.Type == "HasPageII"))
            {
                await _userManager.AddClaimAsync(user, new Claim("HasPageII", Input.HasPageII.ToString()));
            }
            else if (!Input.HasPageII && usersClaims.Any(x => x.Type == "HasPageII"))
            {
                await _userManager.RemoveClaimAsync(user, usersClaims.FirstOrDefault(x => x.Type == "HasPageII"));
            }

        }
    }
}
