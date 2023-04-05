using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Pages;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;

namespace Ahed_project.Services
{

    public class CreateExcelService
    {
        private static ProjectPageViewModel _projectPageViewModel;
        private static TubesFluidViewModel _tubesFluidViewModel;
        private static ShellFluidViewModel _shellFluidViewModel;
        private static HeatBalanceViewModel _heatBalanceViewModel;
        private static GeometryPageViewModel _geometryPageViewModel;
        private static SLDocument Doc;
        public CreateExcelService(TubesFluidViewModel tubesFluidViewModel, ShellFluidViewModel shellFluidViewModel, ProjectPageViewModel projectPageViewModel, HeatBalanceViewModel heatBalanceViewModel, GeometryPageViewModel geometryPageViewModel)
        {
            _projectPageViewModel = projectPageViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;
            _geometryPageViewModel = geometryPageViewModel;

            Doc = new();
        }
        public void CreateExcel()
        {
            CreateFull();
        }

        private static void CreateFull()
        {
            try
            {
                var directory = Directory.GetCurrentDirectory();
                if (File.Exists($"{directory}\\FullReport.xlsx")){
                    File.Delete($"{directory}\\FullReport.xlsx");
                }
                AddTubeData();
                AddShellData();
                AddHeatBalanceData();
                AddGeometryData();
                Doc.SaveAs("FullReport.xlsx");
                var p = new System.Diagnostics.Process();
                p.StartInfo = new ProcessStartInfo($"{directory}\\FullReport.xlsx")
                {
                    UseShellExecute = true,
                };
                p.Start();


            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", $"Message: {e.Message}\r\nExcep: {e}")));
            }

        }

        #region styles
        private static SLStyle BoldTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 11;
            style.Font.Bold = true;
            style.SetWrapText(true);
            return style;
        }
        private static SLStyle BoldHeaderTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 14;
            style.Font.Bold = true;
            style.SetWrapText(true);
            return style;
        }
        private static SLStyle BorderCellsStyle()
        {
            var style = new SLStyle();
            style.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            return style;
        }
        #endregion

        #region tube
        private static void AddTubeData()
        {
            Doc.AddWorksheet("TubeReport");
            Doc.DeleteWorksheet("Sheet1");
            Doc.MergeWorksheetCells("A1", "B1");
            Doc.MergeWorksheetCells("A2", "B2");
            Doc.MergeWorksheetCells("A3", "B3");
            Doc.MergeWorksheetCells("A4", "B4");
            Doc.MergeWorksheetCells("C1", "F1");
            Doc.MergeWorksheetCells("C3", "F3");
            Doc.MergeWorksheetCells("C4", "F4");
            Doc.SetCellValue("A1", "Project name");
            Doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            Doc.SetCellStyle("A1", "A6", BoldTextStyle());
            Doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            Doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            Doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            Doc.SetCellStyle("A4", "F4", BorderCellsStyle());
            Doc.SetColumnWidth("A", "M", 15);
            Doc.SetCellValue("A2", "Revision Nr");
            Doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            Doc.SetCellValue("A3", "Process");
            Doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name); 
            Doc.SetCellValue("A4", "Name");
            Doc.SetCellValue("C4", _tubesFluidViewModel.Product.name);

            CreateHeaders();
            CreateUnits();
            AddData(_tubesFluidViewModel.Product.product_properties);
        }


        private static void CreateHeaders()
        {
            Doc.SetCellValue("A8", "Temperature");
            Doc.SetCellValue("B8", "Density");
            Doc.SetCellValue("C8", "Specific heat");
            Doc.SetCellValue("D8", "Thermal conductivity");
            Doc.SetCellValue("E8", "Consistency index");
            Doc.SetCellValue("F8", "Flow index");
            Doc.SetCellValue("G8", "Latent heat");
            Doc.SetCellValue("H8", "Density Gas");
            Doc.SetCellValue("I8", "Specific heat gas at constant pressure (Cp)");
            Doc.SetCellValue("J8", "Thermal Conductivity Gas");
            Doc.SetCellValue("K8", "Dynamic viscosity gas");
            Doc.SetCellValue("L8", "Vapour pressure");
            Doc.SetCellValue("M8", "Mass Vapour Fraction");

            Doc.SetCellStyle("A8", "M8", BoldTextStyle());
        }
        private static void CreateUnits()
        {
            Doc.SetCellValue("A9", "°C");
            Doc.SetCellValue("B9", "kg/m³");
            Doc.SetCellValue("C9", "kcal/kg·°C");
            Doc.SetCellValue("D9", "kcal/m·hr·°C");
            Doc.SetCellValue("E9", "cP");
            Doc.SetCellValue("F9", "");
            Doc.SetCellValue("G9", "kcal/kg");
            Doc.SetCellValue("H9", "kg/m³");
            Doc.SetCellValue("I9", "kcal/kg·°C");
            Doc.SetCellValue("J9", "kcal/m·hr·°C");
            Doc.SetCellValue("K9", "cP");
            Doc.SetCellValue("L9", "bar-a");
            Doc.SetCellValue("M9", "%");
        }
        private static void AddData(IEnumerable<ProductProperties> values)
        {
            ProductProperties[] properties = values.ToArray();
            for (int i = 0; i < properties.Length; i++)
            {
                Doc.SetCellValue($"A{i + 10}", properties[i].liquid_phase_temperature.ToString());
                Doc.SetCellValue($"B{i + 10}", properties[i].liquid_phase_density.ToString());
                Doc.SetCellValue($"C{i + 10}", properties[i].liquid_phase_specific_heat.ToString());
                Doc.SetCellValue($"D{i + 10}", properties[i].liquid_phase_thermal_conductivity.ToString());
                Doc.SetCellValue($"E{i + 10}", properties[i].liquid_phase_consistency_index.ToString());
                Doc.SetCellValue($"F{i + 10}", properties[i].liquid_phase_f_ind.ToString());
                Doc.SetCellValue($"G{i + 10}", properties[i].liquid_phase_dh.ToString());
                Doc.SetCellValue($"H{i + 10}", properties[i].gas_phase_density.ToString());
                Doc.SetCellValue($"I{i + 10}", properties[i].gas_phase_specific_heat.ToString());
                Doc.SetCellValue($"J{i + 10}", properties[i].gas_phase_thermal_conductivity.ToString());
                Doc.SetCellValue($"K{i + 10}", properties[i].gas_phase_dyn_visc_gas.ToString());
                Doc.SetCellValue($"L{i + 10}", properties[i].gas_phase_p_vap.ToString());
                Doc.SetCellValue($"M{i + 10}", properties[i].gas_phase_vapour_frac.ToString());

            }
            Doc.SetCellStyle("A8", $"M{properties.Length + 9}", BorderCellsStyle());
        }

        #endregion

        #region shell
        private static void AddShellData()
        {
            Doc.AddWorksheet("ShellReport");
            Doc.MergeWorksheetCells("A1", "B1");
            Doc.MergeWorksheetCells("A2", "B2");
            Doc.MergeWorksheetCells("A3", "B3");
            Doc.MergeWorksheetCells("A4", "B4");
            Doc.MergeWorksheetCells("C1", "F1");
            Doc.MergeWorksheetCells("C3", "F3");
            Doc.MergeWorksheetCells("C4", "F4");
            Doc.SetCellValue("A1", "Project name");
            Doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            Doc.SetCellStyle("A1", "A6", BoldTextStyle());
            Doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            Doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            Doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            Doc.SetCellStyle("A4", "F4", BorderCellsStyle());
            Doc.SetColumnWidth("A", "M", 15);
            Doc.SetCellValue("A2", "Revision Nr");
            Doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            Doc.SetCellValue("A3", "Process");
            Doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);
            Doc.SetCellValue("A4", "Name");
            Doc.SetCellValue("C4", _shellFluidViewModel.Product.name);

            CreateHeaders();
            CreateUnits();
            AddData(_shellFluidViewModel.Product.product_properties);
        }
        #endregion

        #region heat balance
        private static void AddHeatBalanceData()
        {
            Doc.AddWorksheet("HeatBalanceReport");
            Doc.MergeWorksheetCells("A1", "B1");
            Doc.MergeWorksheetCells("A2", "B2");
            Doc.MergeWorksheetCells("A3", "B3");
            Doc.MergeWorksheetCells("A4", "B4");
            Doc.MergeWorksheetCells("C1", "F1");
            Doc.MergeWorksheetCells("C3", "F3");
            Doc.MergeWorksheetCells("C4", "F4");
            Doc.SetCellValue("A1", "Project name");
            Doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            Doc.SetCellStyle("A1", "A6", BoldTextStyle());
            Doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            Doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            Doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            Doc.SetColumnWidth("A", "M", 15);
            Doc.SetCellValue("A2", "Revision Nr");
            Doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            Doc.SetCellValue("A3", "Process");
            Doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);

            Doc.MergeWorksheetCells("A5", "F5");
            Doc.SetCellValue("A5", "Heat Balance");
            Doc.SetCellStyle("A5", BoldHeaderTextStyle());

            Doc.MergeWorksheetCells("A5", "F5");
            Doc.MergeWorksheetCells("C6", "D6");
            Doc.MergeWorksheetCells("E6", "F6");
            Doc.SetCellValue("C6", "Tubes side");
            Doc.SetCellValue("E6", "Shell side");

            Doc.SetCellValue("C7", "In");
            Doc.SetCellValue("D7", "Out");
            Doc.SetCellValue("E7", "In");
            Doc.SetCellValue("F7", "Out");
            Doc.SetCellStyle("C6", "F7", BorderCellsStyle());
            Doc.SetCellStyle("C6", "F7", BoldTextStyle());

            AddNames();
            AddUnits();
            AddValues();
        }

        private static void AddUnits()
        {
            Doc.SetCellValue("B11", "kg/hr");
            Doc.SetCellValue("B12", "°C");
            Doc.SetCellValue("B13", "kW");
            Doc.SetCellValue("B14", "bar-a");
            Doc.SetCellValue("B16", "kg/m³");
            Doc.SetCellValue("B17", "kJ/kg•°С");
            Doc.SetCellValue("B18", "W/m•°C");
            Doc.SetCellValue("B19", "cP");
            Doc.SetCellValue("B21", "J/kg");
            Doc.SetCellValue("B23", "kg/m³");
            Doc.SetCellValue("B24", "kJ/kg•°C");
            Doc.SetCellValue("B25", "W/m•°C");
            Doc.SetCellValue("B26", "cP");
            Doc.SetCellValue("B27", "bar-a");
            Doc.SetCellValue("B28", "%");
        }

        private static void AddNames()
        {
            Doc.SetCellValue("A8", "Fluid Name");
            Doc.SetCellValue("A9", "Flow Type");
            Doc.SetCellValue("A10", "Process");
            Doc.SetCellValue("A11", "Flow");
            Doc.SetCellValue("A12", "Temperature");
            Doc.SetCellValue("A13", "Duty");
            Doc.SetCellValue("A14", "Pressure");
            Doc.SetCellValue("A15", "Liquid Phase");
            Doc.SetCellValue("A16", "Density");
            Doc.SetCellValue("A17", "Specific heat");
            Doc.SetCellValue("A18", "Therm. Cond.");
            Doc.SetCellValue("A19", "Consistency index");
            Doc.SetCellValue("A20", "Flow index");
            Doc.SetCellValue("A21", "Latent heat");
            Doc.SetCellValue("A22", "Gas Phase");
            Doc.SetCellValue("A23", "Density Gas");
            Doc.SetCellValue("A24", "Specific heat gas at constant pressure (Cp)");
            Doc.SetCellValue("A25", "Thermal Conductivity Gas");
            Doc.SetCellValue("A26", "Dynamic viscosity gas");
            Doc.SetCellValue("A27", "Vapour pressure");
            Doc.SetCellValue("A28", "Mass Vapour Fraction");
            Doc.SetCellStyle("A8", "F28", BorderCellsStyle());
            Doc.SetCellStyle("A8", "A28", BoldTextStyle());
        }

        private static void AddValues()
        {
            Doc.MergeWorksheetCells("C8", "D8");
            Doc.SetCellValue("C8", _tubesFluidViewModel.Product.name);
            Doc.MergeWorksheetCells("E8", "F8");
            Doc.SetCellValue("E8", _shellFluidViewModel.Product.name);
            Doc.MergeWorksheetCells("C9", "F9");
            Doc.SetCellValue("C9", "Counter current");
            Doc.MergeWorksheetCells("C10", "D10");
            Doc.SetCellValue("C10", _heatBalanceViewModel.Calculation.process_tube);
            Doc.MergeWorksheetCells("E10", "F10");
            Doc.SetCellValue("E10", _heatBalanceViewModel.Calculation.process_shell);
            Doc.MergeWorksheetCells("C11", "D11");
            Doc.SetCellValue("C11", _heatBalanceViewModel.Calculation.flow_tube);
            Doc.MergeWorksheetCells("E11", "F11");
            Doc.SetCellValue("E11", _heatBalanceViewModel.Calculation.flow_shell);
            Doc.SetCellValue("C12", _heatBalanceViewModel.Calculation.temperature_tube_inlet);
            Doc.SetCellValue("D12", _heatBalanceViewModel.Calculation.temperature_tube_outlet);
            Doc.SetCellValue("E12", _heatBalanceViewModel.Calculation.temperature_shell_inlet);
            Doc.SetCellValue("F12", _heatBalanceViewModel.Calculation.temperature_shell_outlet);
            Doc.MergeWorksheetCells("C13", "D13");
            Doc.MergeWorksheetCells("E13", "F13");
            Doc.SetCellValue("C13", _heatBalanceViewModel.Calculation.duty_tube);
            Doc.SetCellValue("E13", _heatBalanceViewModel.Calculation.duty_shell);
            Doc.SetCellValue("C16", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet);
            Doc.SetCellValue("D16", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet);
            Doc.SetCellValue("E16", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet);
            Doc.SetCellValue("F16", _heatBalanceViewModel.Calculation.liquid_density_shell_outlet);
            Doc.SetCellValue("C17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_inlet);
            Doc.SetCellValue("D17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_outlet);
            Doc.SetCellValue("E17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_inlet);
            Doc.SetCellValue("F17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_outlet);
            Doc.SetCellValue("C18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_inlet);
            Doc.SetCellValue("D18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_outlet);
            Doc.SetCellValue("E18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_inlet);
            Doc.SetCellValue("F18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_outlet);
            Doc.SetCellValue("C19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_inlet);
            Doc.SetCellValue("D19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_outlet);
            Doc.SetCellValue("E19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_inlet);
            Doc.SetCellValue("F19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_outlet);
            Doc.SetCellValue("C20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_inlet);
            Doc.SetCellValue("D20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_outlet);
            Doc.SetCellValue("E20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet);
            Doc.SetCellValue("F20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet);
            Doc.SetCellValue("C21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_inlet);
            Doc.SetCellValue("D21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_outlet);
            Doc.SetCellValue("E21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            Doc.SetCellValue("F21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            Doc.SetCellValue("C23", _heatBalanceViewModel.Calculation.gas_density_tube_inlet);
            Doc.SetCellValue("D23", _heatBalanceViewModel.Calculation.gas_density_tube_outlet);
            Doc.SetCellValue("E23", _heatBalanceViewModel.Calculation.gas_density_shell_inlet);
            Doc.SetCellValue("F23", _heatBalanceViewModel.Calculation.gas_density_shell_outlet);
            Doc.SetCellValue("C24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_inlet);
            Doc.SetCellValue("D24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_outlet);
            Doc.SetCellValue("E24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_inlet);
            Doc.SetCellValue("F24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_outlet);
            Doc.SetCellValue("C25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_inlet);
            Doc.SetCellValue("D25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_outlet);
            Doc.SetCellValue("E25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_inlet);
            Doc.SetCellValue("F25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_outlet);
            Doc.SetCellValue("C26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_inlet);
            Doc.SetCellValue("D26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_outlet);
            Doc.SetCellValue("E26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_inlet);
            Doc.SetCellValue("F26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_outlet);
            Doc.SetCellValue("C27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_inlet);
            Doc.SetCellValue("D27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_outlet);
            Doc.SetCellValue("E27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_inlet);
            Doc.SetCellValue("F27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_outlet);
            Doc.SetCellValue("C28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_tube_inlet);
            Doc.SetCellValue("D28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_tube_outlet);
            Doc.SetCellValue("E28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_shell_inlet);
            Doc.SetCellValue("F28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_shell_outlet);
        }
        #endregion

        #region geometry
        private static void AddGeometryData()
        {
            Doc.AddWorksheet("GeometryReport");
            Doc.MergeWorksheetCells("A1", "B1");
            Doc.MergeWorksheetCells("A2", "B2");
            Doc.MergeWorksheetCells("A3", "B3");
            Doc.MergeWorksheetCells("A4", "B4");
            Doc.MergeWorksheetCells("C1", "F1");
            Doc.MergeWorksheetCells("C3", "F3");
            Doc.MergeWorksheetCells("C4", "F4");
            Doc.SetCellValue("A1", "Project name");
            Doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            Doc.SetCellStyle("A1", "A6", BoldTextStyle());
            Doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            Doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            Doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            Doc.SetColumnWidth("A", "M", 15);
            Doc.SetCellValue("A2", "Revision Nr");
            Doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            Doc.SetCellValue("A3", "Process");
            Doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);

            AddGeometryTitle("A6", "E6", "Tube & Shell Geometry");

            Doc.SetCellValue("C7", "Inner Side");
            Doc.SetCellValue("D7", "Tube Side");
            Doc.SetCellValue("E7", "Shell Side");

            Doc.SetCellStyle("C7", "E7", BorderCellsStyle());
            Doc.SetCellStyle("C7", "F7", BoldTextStyle());
            AddGeometryNames();
            AddGeometryTitle("A25", "E25", "Tubeplate Layout");
            AddGeometryTubeplateNames();
            AddGeometryTitle("A37", "E37", "Nozzles");
            AddGeometryNozzlesNames();
            AddGeometryTitle("A50", "E50", "Baffles");
            AddGeometryBafflesNames();
            AddGeometryValues();

            DownloadImage(_geometryPageViewModel.Geometry.image_geometry);

        }

        private static void DownloadImage(string url)
        {
            WebClient client = new();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory($"{path}\\Apora");

            client.DownloadFile(new Uri(_geometryPageViewModel.Geometry.image_geometry), @$"{path}\\Apora\\geometry_image.png");
            SLPicture pic = new SLPicture(@$"{path}\\Apora\\geometry_image.png");
            pic.SetPosition(6, 7);
            Doc.InsertPicture(pic);
        }

        private static void AddGeometryNames()
        {
            Doc.SetCellValue("A8", "Outer Diameter");
            Doc.SetCellValue("A9", "Thickness");
            Doc.SetCellValue("A10", "Inner Diameter");
            Doc.SetCellValue("A11", "Material");
            Doc.SetCellValue("A12", "Number of Tubes");
            Doc.SetCellValue("A13", "Tube Inner Length (Lti)");
            Doc.SetCellValue("A14", "Orientation");
            Doc.SetCellValue("A15", "Wetted Perimeter (per pass)");
            Doc.SetCellValue("A16", "Hydraulic Diameter");
            Doc.SetCellValue("A17", "Area / Module");
            Doc.SetCellValue("A18", "Volume / Module");
            Doc.SetCellValue("A19", "Tube Profile");
            Doc.SetCellValue("A20", "Roughness (ε)");
            Doc.SetCellValue("A21", "Bundle Type");
            Doc.SetCellValue("A22", "Roller Expanded");
            Doc.SetCellStyle("A8", "E22", BorderCellsStyle());
            Doc.SetCellStyle("A8", "A22", BoldTextStyle());
        }

        private static void AddGeometryTubeplateNames()
        {
            Doc.SetCellValue("A26", "Tube Pitch");
            Doc.SetCellValue("A27", "Tube Layout");
            Doc.SetCellValue("A28", "Number of Passes");
            Doc.SetCellValue("A29", "Div. Plate Layout");
            Doc.SetCellValue("A30", "Div. Plate Thickness");
            Doc.SetCellValue("A31", "Flow cross section");
            Doc.SetCellValue("A32", "Perimeter");
            Doc.SetCellValue("A33", "Max. Nr. Tubes");
            Doc.SetCellValue("A34", "Tube Distribution");
            Doc.SetCellValue("A35", "Tube-Tube Spacing");
            Doc.SetCellStyle("A26", "E35", BorderCellsStyle());
            Doc.SetCellStyle("A26", "A35", BoldTextStyle());
        }

        private static void AddGeometryNozzlesNames()
        {
            Doc.SetCellValue("A38", "Inlet nozzle OD");
            Doc.SetCellValue("A39", "Inlet nozzle wall");
            Doc.SetCellValue("A40", "Inlet nozzle ID");
            Doc.SetCellValue("A41", "In Length");
            Doc.SetCellValue("A42", "Outlet nozzle OD");
            Doc.SetCellValue("A43", "Outlet nozzle wall");
            Doc.SetCellValue("A44", "Outlet nozzle ID");
            Doc.SetCellValue("A45", "Out Length");
            Doc.SetCellValue("A46", "Number of Parallel Lines");
            Doc.SetCellValue("A47", "Number of Modules per Block");
            Doc.SetCellValue("A48", "Shell nozzle orientation");
            Doc.SetCellStyle("A38", "E48", BorderCellsStyle());
            Doc.SetCellStyle("A38", "A48", BoldTextStyle());
        }
        private static void AddGeometryBafflesNames()
        {
            Doc.SetCellValue("A51", "Nr. Baffles");
            Doc.SetCellValue("A52", "Baffle cut (% ID)");
            Doc.SetCellValue("A53", "Inlet baffle spacing (Lbi)");
            Doc.SetCellValue("A54", "Central baffle spacing (Lbc)");
            Doc.SetCellValue("A55", "Outlet baffle spacing (Lbo)");
            Doc.SetCellValue("A56", "Baffle thickness");
            Doc.SetCellValue("A57", "Pairs of sealing strips (Nss)");
            Doc.SetCellStyle("A51", "E57", BorderCellsStyle());
            Doc.SetCellStyle("A51", "A57", BoldTextStyle());
        }

        private static void AddGeometryTitle(string position1, string position2, string title)
        {
            Doc.MergeWorksheetCells(position1, position2);
            Doc.SetCellValue(position1, title);
            Doc.SetCellStyle(position1, BoldHeaderTextStyle());
        }

        private static void AddGeometryValues()
        {
            Doc.SetCellValue("C8", _geometryPageViewModel.Geometry.outer_diameter_inner_side);
            Doc.SetCellValue("D8", _geometryPageViewModel.Geometry.outer_diameter_tubes_side);
            Doc.SetCellValue("E8", _geometryPageViewModel.Geometry.outer_diameter_shell_side);
            Doc.SetCellValue("C9", _geometryPageViewModel.Geometry.thickness_inner_side);
            Doc.SetCellValue("D9", _geometryPageViewModel.Geometry.thickness_tubes_side);
            Doc.SetCellValue("E9", _geometryPageViewModel.Geometry.thickness_shell_side);
            Doc.SetCellValue("C10", _geometryPageViewModel.Geometry.inner_diameter_inner_side);
            Doc.SetCellValue("D10", _geometryPageViewModel.Geometry.inner_diameter_tubes_side);
            Doc.SetCellValue("E10", _geometryPageViewModel.Geometry.inner_diameter_shell_side);
            Doc.MergeWorksheetCells("C11", "D11");
            Doc.SetCellValue("C11", _geometryPageViewModel.Geometry.material_tubes_side);
            Doc.SetCellValue("E11", _geometryPageViewModel.Geometry.material_shell_side);
            Doc.SetCellValue("D12", _geometryPageViewModel.Geometry.number_of_tubes);
            Doc.SetCellValue("D13", _geometryPageViewModel.Geometry.tube_inner_length);
            Doc.MergeWorksheetCells("C14", "E14");
            Doc.SetCellValue("D14", _geometryPageViewModel.Geometry.tube_inner_length);
            Doc.SetCellValue("D15", _geometryPageViewModel.Geometry.wetted_perimeter_tubes_side);
            Doc.SetCellValue("E15", _geometryPageViewModel.Geometry.wetted_perimeter_shell_side);
            Doc.SetCellValue("D16", _geometryPageViewModel.Geometry.hydraulic_diameter_tubes_side);
            Doc.SetCellValue("E16", _geometryPageViewModel.Geometry.hydraulic_diameter_shell_side);
            Doc.SetCellValue("D17", _geometryPageViewModel.Geometry.area_module);
            Doc.SetCellValue("D18", _geometryPageViewModel.Geometry.volume_module_tubes_side);
            Doc.SetCellValue("E18", _geometryPageViewModel.Geometry.volume_module_shell_side);
            Doc.MergeWorksheetCells("C19", "D19");
            Doc.SetCellValue("C19", _geometryPageViewModel.Geometry.tube_profile_tubes_side);
            Doc.SetCellValue("D20", _geometryPageViewModel.Geometry.roughness_tubes_side);
            Doc.SetCellValue("E20", _geometryPageViewModel.Geometry.roughness_shell_side);
            Doc.MergeWorksheetCells("C21", "E21");
            Doc.SetCellValue("C21", _geometryPageViewModel.Geometry.bundle_type);
            Doc.SetCellValue("D22", _geometryPageViewModel.Geometry.roller_expanded);
        }

        #endregion

    }
}
