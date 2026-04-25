using System;
using Model;
using ViewModel;

class Program {
    static void Main() {
        User currentUser = UserDB.SelectById(18); // Example user
        UserDB userDb = new UserDB();
        
        int goal = 5;
        currentUser.Goal = goal;
        
        userDb.Update(currentUser);
        int result = userDb.SaveChanges();
        Console.WriteLine("Result from SaveChanges: " + result);
    }
}
