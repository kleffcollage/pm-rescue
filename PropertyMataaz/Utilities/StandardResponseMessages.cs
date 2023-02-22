using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.Utilities
{
    public class StandardResponseMessages
    {
        public static string OK => SUCCESSFUL;
        public static string PASSWORD_RESET_EMAIL_SENT = "An email has been sent to you with instructions and next steps";
        public static string SUCCESSFUL = "Successful";
        public static string UNSUCCESSFUL = "Unsuccessful";
        public static string ERROR_OCCURRED = "An Error Occurred, please try again later";

        public static string INVALID_OLD_PASSWORD = "Please supply a correct password for your old password";
        public static string EMAIL_VERIFIED = "Email verification successful";
        public static string ALREADY_ACTIVATED = "Email already verified";
        public static string EMAIL_VERIFICATION_FAILED =
            "Email verification failed. This Link may have expired please contact admin";
        public static string USER_NOT_FOUND = "User with this email does not exist";
        public static string PASSWORD_RESET_FAILED =
            "Password reset failed. This Link may have expired please contact admin";
        public static string PASSWORD_RESET_COMPLETE = "Your password has been reset successfully";
        public static string USER_ALREADY_EXISTS = "A user with this email already exists.";
        public static string PROPERTY_CREATION_SUCCESSFUL = "Your new Property was created successfully";
        public static string PROPERTY_CREATION_FAILED = "There was an error while creating your property, please try again after sometime. If this persists please contact admin";
        public static string MEDIA_UPLOAD_FAILED = "We encountered an error uploading this media at this time";
        public static string MEDIA_UPLOAD_SUCCESSFUL = "Media Uploaded successfully";
        public static string APPLICATION_NOT_FOUND = "Application by this Id could not be found";
        internal static string DEELETED;
        public static string PAYMENT_INITIATION_FAILED = "THere was an issue initiating this payment, Please try again later or contact admin";
        public static string PAYMENT_UNSUCCESSFUL = "Your payment was unsuccessfull";
        public static string USER_NOT_PERMITTED = "Sorry you are not permitted to log in as an administrator";
        public static string INSPECTION_TIME_UNAVAILABE = "THis particular time has been filled up";
        public static string APPLICATION_ALREADY_APPROVE = "This application has already been approved";
    }
}
