using QRCoder;

namespace SampleAuthentication.Service
{
    public class QRCodeService
    {
        private readonly QRCodeGenerator _generator;

        public QRCodeService(QRCodeGenerator generator)
        {
            _generator = generator;
        }

        public string GetQRCodeAsBase64(string TextToCode)
        {
            QRCodeData qrCodeData = _generator.CreateQrCode(TextToCode,QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);

            return Convert.ToBase64String(qrCode.GetGraphic(4));
        }
    }
}
