using MaxshoesBack.Models.UserModels;

namespace MaxshoesBack.Models.AccountModels
{
    public class EditContactRequest
    {
        public string Email { get; set; }

        public Contact Contact { get; set; }
    }
}