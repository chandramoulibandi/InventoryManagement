using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Interface
{
    internal interface IWorkFlow
    {
        int MainMenu();
        void UpdateNodes();
        void ShowInventorySummary();
    }
}
