using System;
using Model;
using ViewModel;

class Program {
    static void Main() {
        UserDB userDb = new UserDB();
        User u = UserDB.SelectById(18);
        Console.WriteLine("Old goal: " + u.Goal);
        u.Goal = 10;
        userDb.Update(u);
        int res = userDb.SaveChanges();
        Console.WriteLine("Rows affected: " + res);
    }
}
