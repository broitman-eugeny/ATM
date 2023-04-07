using System;
using System.Windows.Forms;

//Класс функции Main() приложения Банкомат
//Банкомат принимает и выдает деньги, отображает своё состояние.
//Купюры могут быть разного достоинства (10,50,100,500, 1000 и 5000 руб.).
//Банкомат должен имеет ограничение по количеству хранимых в нем купюр (в про-грамме заложено для каждого номинала – 1000 шт.). 
//Перед выдачей купюр банкомат спрашивает у пользователя купюрами, какого достоинства произвести выдачу.
class AppATM : FormATM
{
    //public static AppATM appInstance;

    // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
    [STAThread]//Однопоточное приложение (для возможности использования компонентов Windows Forms)
    public static void Main()
    {
        //appInstance = new AppATM();
        //Application.Run(appInstance);
        Application.Run(new AppATM());
    }
}
