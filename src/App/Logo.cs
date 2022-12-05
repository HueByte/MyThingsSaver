namespace MTS.App
{
    public static class Logo
    {
        public static void PrintLogo()
        {

            string title = @"            
       __  __ _____ ____  
      |  \/  |_   _/ ___| 
      | |\/| | | | \___ \ 
      | |  | | | |  ___) |
      |_|  |_| |_| |____/ 
            ";

            string logo = @"             
            @@@@@@@                 
          @@       @@               
     @@@@@           @@%            
   @@     ((           @@@@@        
   @@     ((           @@@@@        
@@@         ((       ((/    @@      
@@@                ((       @@      
@@@((                       @@      
@@@((                       @@              
   @@((.                  @@        
     @@@@@@@@@@@@@@@@@@@@@              
             ";

            Console.WriteLine(title);
            Console.WriteLine(logo);
        }
    }
}