using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ShellClasses;
using Ahed_project.MasterData.TubesClasses;
using Ahed_project.Services.Global.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services.Global.Content
{
    public partial class UnitedStorage:IUnitedStorage
    {
        private ShellInGlobal _shellsData;
        public ShellInGlobal ShellsData
        {
            get => _shellsData;
            set
            {
                _shellsData = value;
            }
        }
        public ShellInGlobal GetShellsData() { return ShellsData; }
        public void UpdateShellsData(ShellInGlobal data) { ShellsData = data; }

        //Выбор продукта Shell
        public void SelectProductShell(int id)
        {
            var product = _allProducts.SelectMany(x=>x.Value).FirstOrDefault(x=>x.product_id== id);
            CalculationData.ShellProductName = product?.name;
            if (Calculation != null && Calculation?.product_id_shell != product?.product_id)
            {
                Calculation.product_id_shell = product?.product_id;
                Task.Factory.StartNew(UpdateCalculationProducts);
            }
            ShellsData.Product = product;
            Validation(true);
        }
    }
}
