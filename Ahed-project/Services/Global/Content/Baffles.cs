using Ahed_project.MasterData;
using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.ViewModel.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services.Global.Content
{
    public partial class UnitedStorage
    {
        public BaffleInGlobal BafflesData { get; set; }
        public BaffleInGlobal GetBafflesData () { return BafflesData; }
        public void SetBafflesData(BaffleInGlobal bafflesData ) { BafflesData = bafflesData;}

        public void SetBaffle(string diametral_clearance_shell_baffle, string diametral_clearance_tube_baffle)
        {
            BafflesData.Baffle.diametral_clearance_shell_baffle = diametral_clearance_shell_baffle;
            BafflesData.Baffle.diametral_clearance_tube_baffle = diametral_clearance_tube_baffle;
        }

        //расчет перегородок
        public async void CalculateBaffle(BaffleFull baffle)
        {
            if (Calculation == null || Calculation.calculation_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string json = JsonConvert.SerializeObject(new
            {
                baffle.type,
                baffle.buffle_cut,
                method = "no_baffles",
                baffle.baffle_cut_direction
            });
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.CALCULATE_BAFFLE, json, Calculation.project_id.ToString(), Calculation.calculation_id.ToString());
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            });
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    var b = JsonConvert.DeserializeObject<BaffleFull>(result.data.ToString());
                    BafflesData.Baffle = b;
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
