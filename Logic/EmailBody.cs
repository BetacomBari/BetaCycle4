namespace BetaCycle4.Logic
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
<head>
</head>
<h1>Reset Password</h1>
<hr>
<p>BetaCycle team from Bari</p>
<p>Tap the button Below to choose a new password</p>
<a href= ""http://localhost:4200/reset?email={email}&code={emailToken}""> Reset Password</a>
<p>Kind Regards</p>
</html>";
        }
    }
}
