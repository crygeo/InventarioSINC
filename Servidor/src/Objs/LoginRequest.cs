namespace Servidor.src.Objs
{
    public class LoginRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class ChangedPasswordRequest
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AsignarRolRequest
    {
        public string UserId { get; set; }
        public string RolId { get; set; }
    }
}

