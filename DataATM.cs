using System;
using System.Windows.Forms;

//Класс, содержащий количество купюр каждого достоинства и функции работы с ними
class DataATM
{
    //Массив состояния банкомата (количество купюр)
    private static int[] stateOfATM = new int[ConstantsATM.NumOfDignities];

    DataATM()
    {
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++)
        {
            stateOfATM[i] = 0;
        }
    }

    //Функция получения состояния банкомата
    public static int[] getBillsFromATM()
    {
        int[] StateOfATM = new int[ConstantsATM.NumOfDignities];
        for (int i = 0; i < ConstantsATM.NumOfDignities; i++ )
        {
            StateOfATM[i]=stateOfATM[i];
        }
        return StateOfATM;
    }
    
    //Делегат, вызывающийся после изменения количества купюр какого-либо достоинства в банкомате
    public delegate void BillsChanged(int digIndex);

    //Функция установки количества купюр заданного достоинства в банкомате и соответствующей метки состояния банкомата
    //digIndex - индекс достоинства купюр
    //amount - количество купюр данного достоинства
    public static void setBillsInATM(int digIndex, int amount, BillsChanged _BillsChanged)
    {        
        if (amount > ConstantsATM.MaxBillsInATM[digIndex])
        {
            MessageBox.Show("Число купюр превышает максимально допустимое", "Купюры " + ConstantsATM.ValuesOfDignities[digIndex].ToString() + " руб.");
            return;
        }
        stateOfATM[digIndex] = amount;
        _BillsChanged(digIndex);//Вызов внешнего обработчика при изменении количества купюр
    }    
}
