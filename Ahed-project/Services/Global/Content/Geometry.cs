using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData;
using Ahed_project.Services.Global.Interface;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using Ahed_project.Services.EF;

namespace Ahed_project.Services.Global.Content
{
    public partial class UnitedStorage:IUnitedStorage
    {
        private GeometryInGlobal _geometryData;
        public GeometryInGlobal GeometryData 
        {
            get => _geometryData;
            set
            {
                _geometryData = value;
            } 
        }

        public GeometryInGlobal GetGeometryData()
        {
            return GeometryData;
        }
        public void SetGeometryData(GeometryInGlobal data)
        {
            GeometryData = data;
        }

        private ObservableCollection<GeometryFull> _geometryCollection { get; set; }
        public ObservableCollection<GeometryFull> GetGeometries()
        {
            return _geometryCollection;
        }
        private bool _geometryCalculated { get; set; }

        public void UpdateGeometry(GeometryFull geometry)
        {
            if (geometry != null)
            {
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == _user.Id);
                    user.LastGeometryId = geometry.geometry_catalog_id;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            GeometryData.Geometry = geometry;
            _geometryCalculated = false;
            Validation(false);
        }

        //расчет геометрии
        public async void CalculateGeometry(GeometryFull geometry)
        {
            if (Calculation == null || Calculation.calculation_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string json = JsonConvert.SerializeObject(new
            {
                geometry.head_exchange_type,
                geometry.name,
                geometry.outer_diameter_inner_side,
                geometry.outer_diameter_tubes_side,
                geometry.outer_diameter_shell_side,
                geometry.thickness_inner_side,
                geometry.thickness_tubes_side,
                geometry.thickness_shell_side,
                geometry.material_tubes_side,
                geometry.material_shell_side,
                geometry.number_of_tubes,
                geometry.tube_inner_length,
                geometry.orientation,
                geometry.tube_profile_tubes_side,
                geometry.roughness_tubes_side,
                geometry.roughness_shell_side,
                geometry.bundle_type,
                geometry.roller_expanded,
                geometry.nozzles_in_outer_diam_inner_side,
                geometry.nozzles_in_outer_diam_tubes_side,
                geometry.nozzles_in_outer_diam_shell_side,
                geometry.nozzles_in_thickness_inner_side,
                geometry.nozzles_in_thickness_tubes_side,
                geometry.nozzles_in_thickness_shell_side,
                geometry.nozzles_in_length_tubes_side,
                geometry.nozzles_in_length_shell_side,
                geometry.nozzles_out_outer_diam_inner_side,
                geometry.nozzles_out_outer_diam_tubes_side,
                geometry.nozzles_out_outer_diam_shell_side,
                geometry.nozzles_out_thickness_inner_side,
                geometry.nozzles_out_thickness_tubes_side,
                geometry.nozzles_out_thickness_shell_side,
                geometry.nozzles_out_length_tubes_side,
                geometry.nozzles_out_length_shell_side,
                geometry.nozzles_number_of_parallel_lines_tubes_side,
                geometry.nozzles_number_of_parallel_lines_shell_side,
                geometry.shell_nozzle_orientation,
                geometry.tube_plate_layout_tube_pitch,
                geometry.tube_plate_layout_tube_layout,
                geometry.tube_plate_layout_number_of_passes,
                geometry.tube_plate_layout_div_plate_layout,
                geometry.tube_plate_layout_sealing_type,
                geometry.tube_plate_layout_housings_space,
                geometry.tube_plate_layout_div_plate_thickness,
                geometry.tube_plate_layout_tubeplate_thickness,
                geometry.scraping_frequency_tubes_side,
                geometry.motor_power_tubes_side,
                geometry.clearances_spacing_tube_to_tubeplate,
                geometry.clearances_spacing_tubeplate_to_shell,
                geometry.clearances_spacing_division_plate_to_shell,
                geometry.clearances_spacing_minimum_tube_hole_to_tubeplate_edge,
                geometry.clearances_spacing_min_tube_hole_to_division_plate_groove,
                geometry.clearances_spacing_division_plate_to_tubeplate,
                geometry.clearances_spacing_minimum_tube_in_tube_spacing
            });
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.CALCULATE_GEOMETRY, json, Calculation.project_id.ToString(), Calculation.calculation_id.ToString());
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
                    var g = JsonConvert.DeserializeObject<GeometryFull>(result.data.ToString());
                    GeometryData.Geometry = g;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            _geometryCalculated = true;
            Validation(true);
        }
    }
}
