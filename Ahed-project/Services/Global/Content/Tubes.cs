using Ahed_project.MasterData.Products;
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
        private TubesInGlobal _tubesData;
        public TubesInGlobal TubesData
        {
            get => _tubesData;
            set
            {
                _tubesData= value;
            }
        }
        public TubesInGlobal GetTubesData() { return TubesData; }
        public void UpdateTubesData(TubesInGlobal tubesData) { TubesData= tubesData; }

        //Выбор продукта Tube
        public void SelectProductTube(int id)
        {
            var product = _allProducts.SelectMany(x => x.Value).FirstOrDefault(x => x.product_id == id);
            CalculationData.TubesProductName = product?.name;
            if (Calculation != null && Calculation?.product_id_tube != product?.product_id)
            {
                Calculation.product_id_tube = product?.product_id;
                Task.Factory.StartNew(UpdateCalculationProducts);
            }
            TubesData.Product = product;
            Validation(true);
        }
    }
}
