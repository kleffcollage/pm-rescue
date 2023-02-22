using System;

namespace PropertyMataaz.Utilities.Constants
{
    public class UtilityConstants
    {
        public static string ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        public static string SendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        public static string GoogleCredentialsFIle = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS_FILE");
        public static string GoogleStorageBucket = Environment.GetEnvironmentVariable("GOOGLE_STORAGE_BUCKET");
        public static string UploadDrive  = Environment.GetEnvironmentVariable("UPLOAD_DRIVE");
        public static string DriveName = Environment.GetEnvironmentVariable("DRIVE_NAME");
        public static string FlutterWaveBaseURL = Environment.GetEnvironmentVariable("FLUTTERWAVE_BASE_URL");
        public static string FlutterWaveAPI_KEY = Environment.GetEnvironmentVariable("FLUTTERWAVE_API_KEY");
        public static string FlutterWaveSEC_KEY = Environment.GetEnvironmentVariable("FLUTTERWAVE_SEC_KEY");
    }
}