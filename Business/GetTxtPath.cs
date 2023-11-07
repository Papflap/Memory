namespace Memory3;

public class GetTxtPath
{
    //simply returns the path of the Highscores.txt file
    //I put it here so when I go back and change the file structure I only have to modify the path once
    public static string getTxtPath()
    {
        return @"C:\Users\Tom\RiderProjects\Memory3\Business\Highscores.txt";
    }
}