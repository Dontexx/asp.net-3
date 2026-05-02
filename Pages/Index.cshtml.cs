using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

public class IndexModel : PageModel
{
    [BindProperty]
    public char[] Board { get; set; }

    public string Status { get; set; } = "";

    public void OnGet()
    {
        if (TempData["board"] != null)
        {
            Board = TempData["board"].ToString().ToCharArray();
        }
        else
        {
            Board = new char[9];
            for (int i = 0; i < 9; i++) Board[i] = ' ';
        }
    }

    public IActionResult OnPost(int move)
    {
        Board = TempData["board"]?.ToString().ToCharArray() ?? new char[9];

        for (int i = 0; i < 9; i++)
            if (Board[i] == '\0') Board[i] = ' ';

        if (Board[move] == ' ')
        {
            Board[move] = 'X';

            if (CheckWin('X'))
            {
                Status = "Ви перемогли!";
                TempData["board"] = new string(Board);
                return Page();
            }

            ComputerMove();

            if (CheckWin('O'))
            {
                Status = "Комп'ютер переміг!";
                TempData["board"] = new string(Board);
                return Page();
            }

            if (IsDraw())
            {
                Status = "Нічия!";
            }
        }

        TempData["board"] = new string(Board);
        return Page();
    }

    void ComputerMove()
    {
        Random rand = new Random();
        int move;

        do
        {
            move = rand.Next(0, 9);
        } while (Board[move] != ' ');

        Board[move] = 'O';
    }

    bool CheckWin(char p)
    {
        int[,] w = {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < 8; i++)
        {
            if (Board[w[i,0]] == p &&
                Board[w[i,1]] == p &&
                Board[w[i,2]] == p)
                return true;
        }
        return false;
    }

    bool IsDraw()
    {
        foreach (var c in Board)
            if (c == ' ') return false;
        return true;
    }
}