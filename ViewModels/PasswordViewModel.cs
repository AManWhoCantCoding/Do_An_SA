using System.ComponentModel.DataAnnotations;

namespace MediSphere.ViewModels
{
    public class PasswordViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
