namespace admin.edaisong.com.Authority
{
    public interface IAuthenticationService
    {
        void SignIn(string data);
        void SignOut();
    }
}
