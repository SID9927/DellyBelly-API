using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Shared.Helpers
{
    public static class EmailTemplateHelper
    {
        public static string SupportEmail { get; set; } = "5065sid@gmail.com";
        public static string SupportPhone { get; set; } = "+91 9927-666062";
        private const string PrimaryColor = "#1C1007"; // Chocolate
        private const string SecondaryColor = "#D4AF37"; // Gold
        private const string BackgroundColor = "#FDFBF7"; // Cream
        private const string AccentColor = "#d2691e"; // Orange-ish

        public static string GetNewsletterTemplate(string content, string subject)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>{subject}</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: {BackgroundColor}; margin: 0; padding: 0; color: {PrimaryColor}; }}
                    .container {{ max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 10px 30px rgba(28, 16, 7, 0.05); border: 1px solid #f0e6d2; }}
                    .header {{ background-color: {PrimaryColor}; padding: 40px 20px; text-align: center; }}
                    .header h1 {{ color: {SecondaryColor}; margin: 0; font-size: 28px; font-weight: 900; letter-spacing: 2px; text-transform: uppercase; }}
                    .hero {{ background: linear-gradient(135deg, {PrimaryColor} 0%, #2d1b18 100%); padding: 30px 20px; text-align: center; color: #ffffff; }}
                    .hero p {{ margin: 0; font-size: 14px; opacity: 0.8; letter-spacing: 1px; text-transform: uppercase; }}
                    .content {{ padding: 50px 40px; line-height: 1.8; font-size: 16px; color: #3e2616; }}
                    .content h2 {{ color: {PrimaryColor}; margin-top: 0; font-size: 24px; font-weight: 800; }}
                    .footer {{ background-color: #f8f5f0; padding: 40px 20px; text-align: center; font-size: 12px; color: #a18e80; border-top: 1px solid #eeebe3; }}
                    .footer p {{ margin: 5px 0; }}
                    .button {{ display: inline-block; padding: 16px 32px; background-color: {PrimaryColor}; color: {SecondaryColor} !important; text-decoration: none; border-radius: 12px; font-weight: 900; font-size: 12px; text-transform: uppercase; letter-spacing: 2px; margin-top: 30px; box-shadow: 0 5px 15px rgba(28, 16, 7, 0.2); transition: transform 0.3s; }}
                    .divider {{ height: 2px; width: 60px; background-color: {SecondaryColor}; margin: 20px auto; border: none; }}
                    .brand-tag {{ font-weight: bold; color: {SecondaryColor}; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>DELLY BELLY</h1>
                    </div>
                    <div class='hero'>
                        <p>Premium Fresh Bakes</p>
                        <div class='divider'></div>
                        <h2 style='color: white; margin: 10px 0;'>{subject}</h2>
                    </div>
                    <div class='content'>
                        {content}
                    </div>
                    <div class='footer'>
                        <p className='brand-tag' style='font-size: 14px; margin-bottom: 20px;'>STAY SWEET.</p>
                        <p>Delly Belly Bakery & Cake Shop</p>
                        <p>Bhopa Road & Jansath Road, Muzaffarnagar</p>
                        <p>Contact: {SupportPhone} | {SupportEmail}</p>
                        <div style='margin-top: 25px;'>
                            <a href='#' style='color: #a18e80; text-decoration: underline;'>Unsubscribe from this list</a>
                        </div>
                    </div>
                </div>
            </body>
            </html>";
        }

        public static string GetContactAdminTemplate(string fullName, string email, string phone, string message)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', sans-serif; color: {PrimaryColor}; line-height: 1.6; }}
                    .box {{ border: 1px solid {SecondaryColor}; border-radius: 16px; padding: 30px; max-width: 500px; background-color: #fff; }}
                    .label {{ font-size: 11px; text-transform: uppercase; font-weight: 900; color: #a18e80; letter-spacing: 1px; }}
                    .value {{ font-size: 16px; font-weight: 600; margin-bottom: 20px; }}
                    .message-box {{ background-color: #fdfaf5; padding: 20px; border-radius: 12px; border-left: 5px solid {SecondaryColor}; }}
                    .header {{ font-size: 20px; font-weight: 800; border-bottom: 2px solid #f0e6d2; padding-bottom: 10px; margin-bottom: 20px; color: {PrimaryColor}; }}
                </style>
            </head>
            <body>
                <div class='box'>
                    <div class='header'>Delly Belly Website Inquiry</div>
                    
                    <div class='label'>Customer Name</div>
                    <div class='value'>{fullName}</div>
                    
                    <div class='label'>Email Address</div>
                    <div class='value'><a href='mailto:{email}' style='color: {AccentColor};'>{email}</a></div>
                    
                    <div class='label'>Phone Number</div>
                    <div class='value'>{phone ?? "Not provided"}</div>
                    
                    <div class='label'>Message Content</div>
                    <div class='message-box'>{message}</div>
                </div>
            </body>
            </html>";
        }

        public static string GetContactUserReceiptTemplate(string fullName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', sans-serif; color: {PrimaryColor}; text-align: center; background-color: {BackgroundColor}; padding: 40px 20px; }}
                    .card {{ max-width: 500px; margin: 0 auto; background: #ffffff; padding: 40px; border-radius: 30px; box-shadow: 0 10px 25px rgba(28, 16, 7, 0.05); border: 1px solid #f0e6d2; }}
                    h2 {{ font-weight: 900; font-size: 24px; margin-bottom: 10px; }}
                    p {{ font-size: 15px; color: #6e5e52; }}
                    .logo {{ color: {SecondaryColor}; font-weight: 900; font-size: 18px; margin-bottom: 30px; letter-spacing: 2px; }}
                </style>
            </head>
            <body>
                <div class='card'>
                    <div class='logo'>DELLY BELLY</div>
                    <h2>Hi {fullName},</h2>
                    <p>We've received your message and our team is already on it! We'll get back to you as soon as the next batch is out of the oven.</p>
                    <p style='margin-top: 30px; font-weight: bold;'>Stay Sweet!<br><span style='color: {SecondaryColor}; font-weight: 900;'>The Delly Belly Team</span></p>
                </div>
            </body>
            </html>";
        }

        public static string GetAdminReplyTemplate(string recipientName, string messageContent)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', sans-serif; color: {PrimaryColor}; background-color: {BackgroundColor}; padding: 40px 20px; }}
                    .card {{ max-width: 600px; margin: 0 auto; background: #ffffff; padding: 40px; border-radius: 30px; box-shadow: 0 10px 25px rgba(28, 16, 7, 0.05); border: 1px solid #f0e6d2; }}
                    .logo {{ color: {PrimaryColor}; font-weight: 900; font-size: 20px; text-align: center; margin-bottom: 30px; letter-spacing: 3px; border-bottom: 1px solid {SecondaryColor}/20; padding-bottom: 15px; }}
                    h2 {{ font-weight: 900; font-size: 22px; margin-bottom: 15px; color: {PrimaryColor}; }}
                    .content {{ font-size: 16px; color: #3e2616; line-height: 1.8; margin-bottom: 30px; white-space: pre-wrap; }}
                    .footer {{ text-align: center; border-top: 1px solid #f0e6d2; padding-top: 25px; margin-top: 20px; }}
                    .sign-off {{ font-weight: bold; color: {PrimaryColor}; font-size: 15px; }}
                    .bakery-name {{ color: {SecondaryColor}; font-weight: 900; text-transform: uppercase; letter-spacing: 1px; }}
                </style>
            </head>
            <body>
                <div class='card'>
                    <div class='logo'>DELLY BELLY</div>
                    <h2>Hi {recipientName},</h2>
                    <div class='content'>
{messageContent}
                    </div>
                    <div class='footer'>
                        <p class='sign-off'>Have a sweet day!<br><span class='bakery-name'>The Delly Belly Team</span></p>
                        <p style='font-size: 11px; color: #a18e80; margin-top: 20px;'>Delly Belly Bakery & Cake Shop | Muzaffarnagar</p>
                    </div>
                </div>
            </body>
            </html>";
        }
        public static string GetPasswordResetTemplate(string fullName, string resetUrl, int expiryMinutes = 15, string portalName = "Admin Portal")
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Reset Your Password — Delly Belly</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: {BackgroundColor}; margin: 0; padding: 0; color: {PrimaryColor}; }}
                    .container {{ max-width: 560px; margin: 40px auto; background: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 10px 30px rgba(28,16,7,0.08); border: 1px solid #f0e6d2; }}
                    .header {{ background-color: {PrimaryColor}; padding: 36px 20px; text-align: center; }}
                    .header h1 {{ color: {SecondaryColor}; margin: 0; font-size: 26px; font-weight: 900; letter-spacing: 3px; text-transform: uppercase; }}
                    .header p {{ color: rgba(212,175,55,0.6); margin: 6px 0 0; font-size: 11px; letter-spacing: 2px; text-transform: uppercase; }}
                    .body {{ padding: 48px 40px; }}
                    .icon-wrap {{ width: 72px; height: 72px; background: #f8f4ee; border-radius: 50%; display: block; line-height: 72px; text-align: center; margin: 0 auto 28px; font-size: 32px; }}
                    h2 {{ font-size: 22px; font-weight: 900; color: {PrimaryColor}; margin: 0 0 12px; }}
                    p {{ font-size: 15px; color: #5c4a3a; line-height: 1.7; margin: 0 0 20px; }}
                    .btn-wrap {{ text-align: center; margin: 36px 0; }}
                    .btn {{ display: inline-block; padding: 16px 40px; background-color: {PrimaryColor}; color: {SecondaryColor} !important; text-decoration: none; border-radius: 14px; font-weight: 900; font-size: 12px; letter-spacing: 2px; text-transform: uppercase; box-shadow: 0 6px 20px rgba(28,16,7,0.2); }}
                    .divider {{ height: 1px; background: #f0e8dc; margin: 32px 0; border: none; }}
                    .note {{ font-size: 13px; color: #a18e80; }}
                    .note a {{ color: {AccentColor}; word-break: break-all; }}
                    .footer {{ background: #f8f5f0; padding: 28px 20px; text-align: center; font-size: 12px; color: #a18e80; border-top: 1px solid #eeebe3; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>DELLY BELLY</h1>
                        <p>{portalName}</p>
                    </div>
                    <div class='body'> 
                        <h2>Reset your password</h2>
                        <p>Hi <strong>{fullName}</strong>,</p>
                        <p>We received a request to reset your Delly Belly account password. Click the button below — this link is valid for <strong>{expiryMinutes} minutes</strong>.</p>
                        <div class='btn-wrap'>
                            <a href='{resetUrl}' class='btn'>Reset Password</a>
                        </div>
                        <hr class='divider' />
                        <p class='note'>If the button doesn't work, paste this link into your browser:<br/><a href='{resetUrl}'>{resetUrl}</a></p>
                        <p class='note'>If you didn't request a password reset, you can safely ignore this email. Your password will remain unchanged.</p>
                    </div>
                    <div class='footer'>
                        <p><strong>Delly Belly Bakery &amp; Cake Shop</strong></p>
                        <p>Bhopa Road &amp; Jansath Road, Muzaffarnagar</p>
                        <p>{SupportPhone} | {SupportEmail}</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        public static string GetWelcomeAdminTemplate(string fullName)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <title>Welcome to Delly Belly Admin</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: {BackgroundColor}; margin: 0; padding: 0; }}
                    .container {{ max-width: 560px; margin: 40px auto; background: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 10px 30px rgba(28,16,7,0.08); border: 1px solid #f0e6d2; }}
                    .header {{ background-color: {PrimaryColor}; padding: 36px 20px; text-align: center; }}
                    .header h1 {{ color: {SecondaryColor}; margin: 0; font-size: 26px; font-weight: 900; letter-spacing: 3px; text-transform: uppercase; }}
                    .body {{ padding: 48px 40px; text-align: center; }}
                    .badge {{ display: inline-block; background: #f8f4ee; border: 2px solid {SecondaryColor}; color: {PrimaryColor}; font-size: 11px; font-weight: 900; letter-spacing: 2px; text-transform: uppercase; padding: 6px 18px; border-radius: 100px; margin-bottom: 24px; }}
                    h2 {{ font-size: 24px; font-weight: 900; color: {PrimaryColor}; margin: 0 0 16px; }}
                    p {{ font-size: 15px; color: #5c4a3a; line-height: 1.7; margin: 0 0 16px; }}
                    .footer {{ background: #f8f5f0; padding: 28px 20px; text-align: center; font-size: 12px; color: #a18e80; border-top: 1px solid #eeebe3; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'><h1>DELLY BELLY</h1></div>
                    <div class='body'>
                        <div class='badge'>Staff Access</div>
                        <h2>Welcome, {fullName}!</h2>
                        <p>Your Delly Belly admin account has been created successfully.</p>
                        <p>You've been assigned the <strong>Staff</strong> role. A Super Admin can promote your access level as needed.</p>
                        <p style='margin-top:30px; font-weight: bold; color: {PrimaryColor};'>Stay Sweet!<br/><span style='color:{SecondaryColor};'>The Delly Belly Team</span></p>
                    </div>
                    <div class='footer'>
                        <p><strong>Delly Belly Bakery &amp; Cake Shop</strong></p>
                        <p>Bhopa Road &amp; Jansath Road, Muzaffarnagar</p>
                    </div>
                </div>
            </body>
            </html>";
        }
        public static string GetOtpVerificationTemplate(string fullName, string otp)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <title>Verify Your Delly Belly Account</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: {BackgroundColor}; margin: 0; padding: 0; }}
                    .container {{ max-width: 560px; margin: 40px auto; background: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 10px 30px rgba(28,16,7,0.08); border: 1px solid #f0e6d2; }}
                    .header {{ background-color: {PrimaryColor}; padding: 36px 20px; text-align: center; }}
                    .header h1 {{ color: {SecondaryColor}; margin: 0; font-size: 26px; font-weight: 900; letter-spacing: 3px; text-transform: uppercase; }}
                    .body {{ padding: 48px 40px; text-align: center; }}
                    .otp-box {{ display: inline-block; background: #f8f4ee; border: 2px dashed {SecondaryColor}; color: {PrimaryColor}; font-size: 32px; font-weight: 900; letter-spacing: 6px; padding: 16px 32px; border-radius: 12px; margin: 24px 0; }}
                    h2 {{ font-size: 24px; font-weight: 900; color: {PrimaryColor}; margin: 0 0 16px; }}
                    p {{ font-size: 15px; color: #5c4a3a; line-height: 1.7; margin: 0 0 16px; }}
                    .footer {{ background: #f8f5f0; padding: 28px 20px; text-align: center; font-size: 12px; color: #a18e80; border-top: 1px solid #eeebe3; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'><h1>DELLY BELLY</h1></div>
                    <div class='body'>
                        <h2>Verify Your Email</h2>
                        <p>Hi {fullName},</p>
                        <p>Thank you for registering! Please use the following 6-digit code to verify your email address. This code will expire in 10 minutes.</p>
                        <div class='otp-box'>{otp}</div>
                        <p>If you didn't request this, you can safely ignore this email.</p>
                        <p style='margin-top:30px; font-weight: bold; color: {PrimaryColor};'>Stay Sweet!<br/><span style='color:{SecondaryColor};'>The Delly Belly Team</span></p>
                    </div>
                    <div class='footer'>
                        <p><strong>Delly Belly Bakery &amp; Cake Shop</strong></p>
                    </div>
                </div>
            </body>
            </html>";
        }
        public static string GetPasswordResetSuccessTemplate(string fullName)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <title>Password Reset Successful — Delly Belly</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: {BackgroundColor}; margin: 0; padding: 0; color: {PrimaryColor}; }}
                    .container {{ max-width: 560px; margin: 40px auto; background: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 10px 30px rgba(28,16,7,0.08); border: 1px solid #f0e6d2; }}
                    .header {{ background-color: {PrimaryColor}; padding: 36px 20px; text-align: center; }}
                    .header h1 {{ color: {SecondaryColor}; margin: 0; font-size: 26px; font-weight: 900; letter-spacing: 3px; text-transform: uppercase; }}
                    .body {{ padding: 48px 40px; text-align: center; }}
                    .icon {{ font-size: 48px; margin-bottom: 24px; display: block; }}
                    h2 {{ font-size: 24px; font-weight: 900; color: {PrimaryColor}; margin: 0 0 16px; }}
                    p {{ font-size: 15px; color: #5c4a3a; line-height: 1.7; margin: 0 0 24px; }}
                    .footer {{ background: #f8f5f0; padding: 28px 20px; text-align: center; font-size: 12px; color: #a18e80; border-top: 1px solid #eeebe3; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'><h1>DELLY BELLY</h1></div>
                    <div class='body'> 
                        <h2>Password Updated!</h2>
                        <p>Hi <strong>{fullName}</strong>,</p>
                        <p>Your password for the Delly Belly account has been successfully updated. You can now use your new password to sign in.</p>
                        <p style='font-size: 13px; color: #a18e80;'>If you did not make this change, please contact our support team immediately at {SupportEmail}.</p>
                        <p style='margin-top:30px; font-weight: bold; color: {PrimaryColor};'>Stay Sweet!<br/><span style='color:{SecondaryColor};'>The Delly Belly Team</span></p>
                    </div>
                    <div class='footer'>
                        <p><strong>Delly Belly Bakery &amp; Cake Shop</strong></p>
                    </div>
                </div>
            </body>
            </html>";
        }

        public static string GetWelcomeTemplate(string fullName)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <title>Welcome to the Delly Belly Family!</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: {BackgroundColor}; margin: 0; padding: 0; }}
                    .container {{ max-width: 560px; margin: 40px auto; background: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 10px 30px rgba(28,16,7,0.08); border: 1px solid #f0e6d2; }}
                    .header {{ background-color: {PrimaryColor}; padding: 36px 20px; text-align: center; }}
                    .header h1 {{ color: {SecondaryColor}; margin: 0; font-size: 26px; font-weight: 900; letter-spacing: 3px; text-transform: uppercase; }}
                    .body {{ padding: 48px 40px; text-align: center; }}
                    .welcome-badge {{ display: inline-block; background: #f8f4ee; border: 2px solid {SecondaryColor}; color: {PrimaryColor}; font-size: 11px; font-weight: 900; letter-spacing: 2px; text-transform: uppercase; padding: 6px 18px; border-radius: 100px; margin-bottom: 24px; }}
                    h2 {{ font-size: 24px; font-weight: 900; color: {PrimaryColor}; margin: 0 0 16px; }}
                    p {{ font-size: 15px; color: #5c4a3a; line-height: 1.7; margin: 0 0 16px; }}
                    .footer {{ background: #f8f5f0; padding: 28px 20px; text-align: center; font-size: 12px; color: #a18e80; border-top: 1px solid #eeebe3; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'><h1>DELLY BELLY</h1></div>
                    <div class='body'>
                        <div class='welcome-badge'>Welcome</div>
                        <h2>Hi {fullName},</h2>
                        <p>We're so excited to have you join our sweet family! Your account has been created successfully.</p>
                        <p>Now you're all set to browse our premium collection, place orders, and satisfy those cravings.</p>
                        <p style='margin-top:30px; font-weight: bold; color: {PrimaryColor};'>Stay Sweet!<br/><span style='color:{SecondaryColor};'>The Delly Belly Team</span></p>
                    </div>
                    <div class='footer'>
                        <p><strong>Delly Belly Bakery &amp; Cake Shop</strong></p>
                        <p>Bhopa Road &amp; Jansath Road, Muzaffarnagar</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}

