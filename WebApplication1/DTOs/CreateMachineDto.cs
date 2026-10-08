using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateMachineDto
    {//dto thallik tiddeciedi xinfomazzjoni se thalli l user jibat u jircievi, l id nehhejnija, 
        //mghandix tkun xi haga li tista tigi kkontrollata mil client, fil kaz li kellna fields ohra, ez. password, mobile number etc..
        //li ma rridux nuru mal get, narawa iktar id differenza


        //hide confidential information, simplify responses, define different api contract for different operations
        //let us change database structure without breaking clients

        //*

        [Required]//makes value mandatory
        [StringLength(100, MinimumLength = 1)]//ghandna tkun ittra wahda jew iktar, mux iktar min 100
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}