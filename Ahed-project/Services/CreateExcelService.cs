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
        private static BafflesPageViewModel _bafflesPageViewModel;
        private static OverallCalculationViewModel _overallCalculationViewModel;
        public CreateExcelService(TubesFluidViewModel tubesFluidViewModel, ShellFluidViewModel shellFluidViewModel, ProjectPageViewModel projectPageViewModel, HeatBalanceViewModel heatBalanceViewModel, GeometryPageViewModel geometryPageViewModel, BafflesPageViewModel bafflesPageViewModel, OverallCalculationViewModel overallCalculationViewModel)
        {
            _projectPageViewModel = projectPageViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;
            _geometryPageViewModel = geometryPageViewModel;
            _bafflesPageViewModel = bafflesPageViewModel;
            _overallCalculationViewModel = overallCalculationViewModel;
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
                using (var doc = new SLDocument())
                {
                    if (File.Exists($"{directory}\\FullReport.xlsx"))
                    {
                        File.Delete($"{directory}\\FullReport.xlsx");
                    }
                    AddTubeData(doc);
                    AddShellData(doc);
                    AddHeatBalanceData(doc);
                    AddGeometryData(doc);
                    AddOverallData(doc);
                    doc.SaveAs("FullReport.xlsx");
                }
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
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetWrapText(true);
            return style;
        }
        #endregion

        #region tube
        private static void AddTubeData(SLDocument doc)
        {
            doc.AddWorksheet("TubeReport");
            doc.DeleteWorksheet("Sheet1");
            doc.MergeWorksheetCells("A1", "B1");
            doc.MergeWorksheetCells("A2", "B2");
            doc.MergeWorksheetCells("A3", "B3");
            doc.MergeWorksheetCells("A4", "B4");
            doc.MergeWorksheetCells("C1", "F1");
            doc.MergeWorksheetCells("C3", "F3");
            doc.MergeWorksheetCells("C4", "F4");
            doc.SetCellValue("A1", "Project name");
            doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            doc.SetCellStyle("A1", "A6", BoldTextStyle());
            doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            doc.SetCellStyle("A4", "F4", BorderCellsStyle());
            doc.SetColumnWidth("A", "M", 15);
            doc.SetCellValue("A2", "Revision Nr");
            doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("A3", "Process");
            doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name); 
            doc.SetCellValue("A4", "Name");
            doc.SetCellValue("C4", _tubesFluidViewModel.Product.name);

            CreateHeaders(doc);
            CreateUnits(doc);
            AddData(_tubesFluidViewModel.Product.product_properties,doc);
        }


        private static void CreateHeaders(SLDocument doc)
        {
            doc.SetCellValue("A8", "Temperature");
            doc.SetCellValue("B8", "Density");
            doc.SetCellValue("C8", "Specific heat");
            doc.SetCellValue("D8", "Thermal conductivity");
            doc.SetCellValue("E8", "Consistency index");
            doc.SetCellValue("F8", "Flow index");
            doc.SetCellValue("G8", "Latent heat");
            doc.SetCellValue("H8", "Density Gas");
            doc.SetCellValue("I8", "Specific heat gas at constant pressure (Cp)");
            doc.SetCellValue("J8", "Thermal Conductivity Gas");
            doc.SetCellValue("K8", "Dynamic viscosity gas");
            doc.SetCellValue("L8", "Vapour pressure");
            doc.SetCellValue("M8", "Mass Vapour Fraction");

            doc.SetCellStyle("A8", "M8", BoldTextStyle());
        }
        private static void CreateUnits(SLDocument doc)
        {
            doc.SetCellValue("A9", "°C");
            doc.SetCellValue("B9", "kg/m³");
            doc.SetCellValue("C9", "kcal/kg·°C");
            doc.SetCellValue("D9", "kcal/m·hr·°C");
            doc.SetCellValue("E9", "cP");
            doc.SetCellValue("F9", "");
            doc.SetCellValue("G9", "kcal/kg");
            doc.SetCellValue("H9", "kg/m³");
            doc.SetCellValue("I9", "kcal/kg·°C");
            doc.SetCellValue("J9", "kcal/m·hr·°C");
            doc.SetCellValue("K9", "cP");
            doc.SetCellValue("L9", "bar-a");
            doc.SetCellValue("M9", "%");
        }
        private static void AddData(IEnumerable<ProductProperties> values, SLDocument doc)
        {
            ProductProperties[] properties = values.ToArray();
            for (int i = 0; i < properties.Length; i++)
            {
                doc.SetCellValue($"A{i + 10}", properties[i].liquid_phase_temperature.ToString());
                doc.SetCellValue($"B{i + 10}", properties[i].liquid_phase_density.ToString());
                doc.SetCellValue($"C{i + 10}", properties[i].liquid_phase_specific_heat.ToString());
                doc.SetCellValue($"D{i + 10}", properties[i].liquid_phase_thermal_conductivity.ToString());
                doc.SetCellValue($"E{i + 10}", properties[i].liquid_phase_consistency_index.ToString());
                doc.SetCellValue($"F{i + 10}", properties[i].liquid_phase_f_ind.ToString());
                doc.SetCellValue($"G{i + 10}", properties[i].liquid_phase_dh.ToString());
                doc.SetCellValue($"H{i + 10}", properties[i].gas_phase_density.ToString());
                doc.SetCellValue($"I{i + 10}", properties[i].gas_phase_specific_heat.ToString());
                doc.SetCellValue($"J{i + 10}", properties[i].gas_phase_thermal_conductivity.ToString());
                doc.SetCellValue($"K{i + 10}", properties[i].gas_phase_dyn_visc_gas.ToString());
                doc.SetCellValue($"L{i + 10}", properties[i].gas_phase_p_vap.ToString());
                doc.SetCellValue($"M{i + 10}", properties[i].gas_phase_vapour_frac.ToString());

            }
            doc.SetCellStyle("A8", $"M{properties.Length + 9}", BorderCellsStyle());
        }

        #endregion

        #region shell
        private static void AddShellData(SLDocument doc)
        {
            doc.AddWorksheet("ShellReport");
            doc.MergeWorksheetCells("A1", "B1");
            doc.MergeWorksheetCells("A2", "B2");
            doc.MergeWorksheetCells("A3", "B3");
            doc.MergeWorksheetCells("A4", "B4");
            doc.MergeWorksheetCells("C1", "F1");
            doc.MergeWorksheetCells("C3", "F3");
            doc.MergeWorksheetCells("C4", "F4");
            doc.SetCellValue("A1", "Project name");
            doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            doc.SetCellStyle("A1", "A6", BoldTextStyle());
            doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            doc.SetCellStyle("A4", "F4", BorderCellsStyle());
            doc.SetColumnWidth("A", "M", 15);
            doc.SetCellValue("A2", "Revision Nr");
            doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("A3", "Process");
            doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);
            doc.SetCellValue("A4", "Name");
            doc.SetCellValue("C4", _shellFluidViewModel.Product.name);

            CreateHeaders(doc);
            CreateUnits(doc);
            AddData(_shellFluidViewModel.Product.product_properties, doc);
        }
        #endregion

        #region heat balance
        private static void AddHeatBalanceData(SLDocument doc)
        {
            doc.AddWorksheet("HeatBalanceReport");
            doc.MergeWorksheetCells("A1", "B1");
            doc.MergeWorksheetCells("A2", "B2");
            doc.MergeWorksheetCells("A3", "B3");
            doc.MergeWorksheetCells("A4", "B4");
            doc.MergeWorksheetCells("C1", "F1");
            doc.MergeWorksheetCells("C3", "F3");
            doc.MergeWorksheetCells("C4", "F4");
            doc.SetCellValue("A1", "Project name");
            doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            doc.SetCellStyle("A1", "A6", BoldTextStyle());
            doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            doc.SetColumnWidth("A", "M", 15);
            doc.SetCellValue("A2", "Revision Nr");
            doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("A3", "Process");
            doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);

            doc.MergeWorksheetCells("A5", "F5");
            doc.SetCellValue("A5", "Heat Balance");
            doc.SetCellStyle("A5", BoldHeaderTextStyle());

            doc.MergeWorksheetCells("A5", "F5");
            doc.MergeWorksheetCells("C6", "D6");
            doc.MergeWorksheetCells("E6", "F6");
            doc.SetCellValue("C6", "Tubes side");
            doc.SetCellValue("E6", "Shell side");

            doc.SetCellValue("C7", "In");
            doc.SetCellValue("D7", "Out");
            doc.SetCellValue("E7", "In");
            doc.SetCellValue("F7", "Out");
            doc.SetCellStyle("C6", "F7", BorderCellsStyle());
            doc.SetCellStyle("C6", "F7", BoldTextStyle());

            AddNames(doc);
            AddUnits(doc);
            AddValues(doc);
        }

        private static void AddUnits(SLDocument doc)
        {
            doc.SetCellValue("B11", "kg/hr");
            doc.SetCellValue("B12", "°C");
            doc.SetCellValue("B13", "kW");
            doc.SetCellValue("B14", "bar-a");
            doc.SetCellValue("B16", "kg/m³");
            doc.SetCellValue("B17", "kJ/kg•°С");
            doc.SetCellValue("B18", "W/m•°C");
            doc.SetCellValue("B19", "cP");
            doc.SetCellValue("B21", "J/kg");
            doc.SetCellValue("B23", "kg/m³");
            doc.SetCellValue("B24", "kJ/kg•°C");
            doc.SetCellValue("B25", "W/m•°C");
            doc.SetCellValue("B26", "cP");
            doc.SetCellValue("B27", "bar-a");
            doc.SetCellValue("B28", "%");
        }

        private static void AddNames(SLDocument doc)
        {
            doc.SetCellValue("A8", "Fluid Name");
            doc.SetCellValue("A9", "Flow Type");
            doc.SetCellValue("A10", "Process");
            doc.SetCellValue("A11", "Flow");
            doc.SetCellValue("A12", "Temperature");
            doc.SetCellValue("A13", "Duty");
            doc.SetCellValue("A14", "Pressure");
            doc.SetCellValue("A15", "Liquid Phase");
            doc.SetCellValue("A16", "Density");
            doc.SetCellValue("A17", "Specific heat");
            doc.SetCellValue("A18", "Therm. Cond.");
            doc.SetCellValue("A19", "Consistency index");
            doc.SetCellValue("A20", "Flow index");
            doc.SetCellValue("A21", "Latent heat");
            doc.SetCellValue("A22", "Gas Phase");
            doc.SetCellValue("A23", "Density Gas");
            doc.SetCellValue("A24", "Specific heat gas at constant pressure (Cp)");
            doc.SetCellValue("A25", "Thermal Conductivity Gas");
            doc.SetCellValue("A26", "Dynamic viscosity gas");
            doc.SetCellValue("A27", "Vapour pressure");
            doc.SetCellValue("A28", "Mass Vapour Fraction");
            doc.SetCellStyle("A8", "F28", BorderCellsStyle());
            doc.SetCellStyle("A8", "A28", BoldTextStyle());
        }

        private static void AddValues(SLDocument doc)
        {
            doc.MergeWorksheetCells("C8", "D8");
            doc.SetCellValue("C8", _tubesFluidViewModel.Product.name);
            doc.MergeWorksheetCells("E8", "F8");
            doc.SetCellValue("E8", _shellFluidViewModel.Product.name);
            doc.MergeWorksheetCells("C9", "F9");
            doc.SetCellValue("C9", "Counter current");
            doc.MergeWorksheetCells("C10", "D10");
            doc.SetCellValue("C10", _heatBalanceViewModel.Calculation.process_tube);
            doc.MergeWorksheetCells("E10", "F10");
            doc.SetCellValue("E10", _heatBalanceViewModel.Calculation.process_shell);
            doc.MergeWorksheetCells("C11", "D11");
            doc.SetCellValue("C11", _heatBalanceViewModel.Calculation.flow_tube);
            doc.MergeWorksheetCells("E11", "F11");
            doc.SetCellValue("E11", _heatBalanceViewModel.Calculation.flow_shell);
            doc.SetCellValue("C12", _heatBalanceViewModel.Calculation.temperature_tube_inlet);
            doc.SetCellValue("D12", _heatBalanceViewModel.Calculation.temperature_tube_outlet);
            doc.SetCellValue("E12", _heatBalanceViewModel.Calculation.temperature_shell_inlet);
            doc.SetCellValue("F12", _heatBalanceViewModel.Calculation.temperature_shell_outlet);
            doc.MergeWorksheetCells("C13", "D13");
            doc.MergeWorksheetCells("E13", "F13");
            doc.SetCellValue("C13", _heatBalanceViewModel.Calculation.duty_tube);
            doc.SetCellValue("E13", _heatBalanceViewModel.Calculation.duty_shell);
            doc.SetCellValue("C16", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet);
            doc.SetCellValue("D16", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet);
            doc.SetCellValue("E16", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet);
            doc.SetCellValue("F16", _heatBalanceViewModel.Calculation.liquid_density_shell_outlet);
            doc.SetCellValue("C17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_inlet);
            doc.SetCellValue("D17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_outlet);
            doc.SetCellValue("E17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_inlet);
            doc.SetCellValue("F17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_outlet);
            doc.SetCellValue("C18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_inlet);
            doc.SetCellValue("D18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_outlet);
            doc.SetCellValue("E18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_inlet);
            doc.SetCellValue("F18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_outlet);
            doc.SetCellValue("C19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_inlet);
            doc.SetCellValue("D19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_outlet);
            doc.SetCellValue("E19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_inlet);
            doc.SetCellValue("F19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_outlet);
            doc.SetCellValue("C20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_inlet);
            doc.SetCellValue("D20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_outlet);
            doc.SetCellValue("E20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet);
            doc.SetCellValue("F20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet);
            doc.SetCellValue("C21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_inlet);
            doc.SetCellValue("D21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_outlet);
            doc.SetCellValue("E21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            doc.SetCellValue("F21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            doc.SetCellValue("C23", _heatBalanceViewModel.Calculation.gas_density_tube_inlet);
            doc.SetCellValue("D23", _heatBalanceViewModel.Calculation.gas_density_tube_outlet);
            doc.SetCellValue("E23", _heatBalanceViewModel.Calculation.gas_density_shell_inlet);
            doc.SetCellValue("F23", _heatBalanceViewModel.Calculation.gas_density_shell_outlet);
            doc.SetCellValue("C24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_inlet);
            doc.SetCellValue("D24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_outlet);
            doc.SetCellValue("E24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_inlet);
            doc.SetCellValue("F24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_outlet);
            doc.SetCellValue("C25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_inlet);
            doc.SetCellValue("D25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_outlet);
            doc.SetCellValue("E25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_inlet);
            doc.SetCellValue("F25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_outlet);
            doc.SetCellValue("C26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_inlet);
            doc.SetCellValue("D26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_outlet);
            doc.SetCellValue("E26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_inlet);
            doc.SetCellValue("F26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_outlet);
            doc.SetCellValue("C27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_inlet);
            doc.SetCellValue("D27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_outlet);
            doc.SetCellValue("E27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_inlet);
            doc.SetCellValue("F27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_outlet);
            doc.SetCellValue("C28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_tube_inlet);
            doc.SetCellValue("D28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_tube_outlet);
            doc.SetCellValue("E28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_shell_inlet);
            doc.SetCellValue("F28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_shell_outlet);
        }
        #endregion

        #region geometry
        private static void AddGeometryData(SLDocument doc)
        {
            doc.AddWorksheet("GeometryReport");
            doc.MergeWorksheetCells("A1", "B1");
            doc.MergeWorksheetCells("A2", "B2");
            doc.MergeWorksheetCells("A3", "B3");
            doc.MergeWorksheetCells("A4", "B4");
            doc.MergeWorksheetCells("C1", "F1");
            doc.MergeWorksheetCells("C3", "F3");
            doc.MergeWorksheetCells("C4", "F4");
            doc.SetCellValue("A1", "Project name");
            doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            doc.SetCellStyle("A1", "A6", BoldTextStyle());
            doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            doc.SetColumnWidth("A", "M", 15);
            doc.SetCellValue("A2", "Revision Nr");
            doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("A3", "Process");
            doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);

            AddGeometryTitle("A6", "E6", "Tube & Shell Geometry",doc);

            doc.SetCellValue("C7", "Inner Side");
            doc.SetCellValue("D7", "Tube Side");
            doc.SetCellValue("E7", "Shell Side");

            doc.SetCellStyle("C7", "E7", BorderCellsStyle());
            doc.SetCellStyle("C7", "F7", BoldTextStyle());
            AddGeometryNames(doc);
            AddGeometryTitle("A25", "E25", "Tubeplate Layout",doc);
            AddGeometryTubeplateNames(doc);
            AddGeometryTitle("A37", "E37", "Nozzles",doc);
            AddGeometryNozzlesNames(doc);
            AddGeometryTitle("A50", "E50", "Baffles",doc);
            AddGeometryBafflesNames(doc);
            AddGeometryValues(doc);
            AddGeometryUnits(doc);

            DownloadImage(_geometryPageViewModel.Geometry.image_geometry,doc);

        }

        private static void DownloadImage(string url, SLDocument doc)
        {
            WebClient client = new();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory($"{path}\\Apora");

            client.DownloadFile(new Uri(_geometryPageViewModel.Geometry.image_geometry), @$"{path}\\Apora\\geometry_image.png");
            SLPicture pic = new SLPicture(@$"{path}\\Apora\\geometry_image.png");
            pic.ResizeInPixels(300, 300);
            pic.SetPosition(6, 5);
            doc.InsertPicture(pic);
        }

        private static void AddGeometryNames(SLDocument doc)
        {
            doc.SetCellValue("A8", "Outer Diameter");
            doc.SetCellValue("A9", "Thickness");
            doc.SetCellValue("A10", "Inner Diameter");
            doc.SetCellValue("A11", "Material");
            doc.SetCellValue("A12", "Number of Tubes");
            doc.SetCellValue("A13", "Tube Inner Length (Lti)");
            doc.SetCellValue("A14", "Orientation");
            doc.SetCellValue("A15", "Wetted Perimeter (per pass)");
            doc.SetCellValue("A16", "Hydraulic Diameter");
            doc.SetCellValue("A17", "Area / Module");
            doc.SetCellValue("A18", "Volume / Module");
            doc.SetCellValue("A19", "Tube Profile");
            doc.SetCellValue("A20", "Roughness (ε)");
            doc.SetCellValue("A21", "Bundle Type");
            doc.SetCellValue("A22", "Roller Expanded");
            doc.SetCellStyle("A8", "E22", BorderCellsStyle());
            doc.SetCellStyle("A8", "A22", BoldTextStyle());
        }

        private static void AddGeometryTubeplateNames(SLDocument doc)
        {
            doc.SetCellValue("A26", "Tube Pitch");
            doc.SetCellValue("A27", "Tube Layout");
            doc.SetCellValue("A28", "Number of Passes");
            doc.SetCellValue("A29", "Div. Plate Layout");
            doc.SetCellValue("A30", "Div. Plate Thickness");
            doc.SetCellValue("A31", "Flow cross section");
            doc.SetCellValue("A32", "Perimeter");
            doc.SetCellValue("A33", "Max. Nr. Tubes");
            doc.SetCellValue("A34", "Tube Distribution");
            doc.SetCellValue("A35", "Tube-Tube Spacing");
            doc.SetCellStyle("A26", "E35", BorderCellsStyle());
            doc.SetCellStyle("A26", "A35", BoldTextStyle());
        }

        private static void AddGeometryNozzlesNames(SLDocument doc)
        {
            doc.SetCellValue("A38", "Inlet nozzle OD");
            doc.SetCellValue("A39", "Inlet nozzle wall");
            doc.SetCellValue("A40", "Inlet nozzle ID");
            doc.SetCellValue("A41", "In Length");
            doc.SetCellValue("A42", "Outlet nozzle OD");
            doc.SetCellValue("A43", "Outlet nozzle wall");
            doc.SetCellValue("A44", "Outlet nozzle ID");
            doc.SetCellValue("A45", "Out Length");
            doc.SetCellValue("A46", "Number of Parallel Lines");
            doc.SetCellValue("A47", "Number of Modules per Block");
            doc.SetCellValue("A48", "Shell nozzle orientation");
            doc.SetCellStyle("A38", "E48", BorderCellsStyle());
            doc.SetCellStyle("A38", "A48", BoldTextStyle());
        }
        private static void AddGeometryBafflesNames(SLDocument doc)
        {
            doc.SetCellValue("A51", "Nr. Baffles");
            doc.SetCellValue("A52", "Baffle cut (% ID)");
            doc.SetCellValue("A53", "Inlet baffle spacing (Lbi)");
            doc.SetCellValue("A54", "Central baffle spacing (Lbc)");
            doc.SetCellValue("A55", "Outlet baffle spacing (Lbo)");
            doc.SetCellValue("A56", "Baffle thickness");
            doc.SetCellValue("A57", "Pairs of sealing strips (Nss)");
            doc.SetCellStyle("A51", "E57", BorderCellsStyle());
            doc.SetCellStyle("A51", "A57", BoldTextStyle());
        }

        private static void AddGeometryTitle(string position1, string position2, string title, SLDocument doc)
        {
            doc.MergeWorksheetCells(position1, position2);
            doc.SetCellValue(position1, title);
            doc.SetCellStyle(position1, BoldHeaderTextStyle());
        }

        private static void AddGeometryValues(SLDocument doc)
        {
            doc.SetCellValue("C8", _geometryPageViewModel.Geometry.outer_diameter_inner_side);
            doc.SetCellValue("D8", _geometryPageViewModel.Geometry.outer_diameter_tubes_side);
            doc.SetCellValue("E8", _geometryPageViewModel.Geometry.outer_diameter_shell_side);
            doc.SetCellValue("C9", _geometryPageViewModel.Geometry.thickness_inner_side);
            doc.SetCellValue("D9", _geometryPageViewModel.Geometry.thickness_tubes_side);
            doc.SetCellValue("E9", _geometryPageViewModel.Geometry.thickness_shell_side);
            doc.SetCellValue("C10", _geometryPageViewModel.Geometry.inner_diameter_inner_side);
            doc.SetCellValue("D10", _geometryPageViewModel.Geometry.inner_diameter_tubes_side);
            doc.SetCellValue("E10", _geometryPageViewModel.Geometry.inner_diameter_shell_side);
            doc.MergeWorksheetCells("C11", "D11");
            doc.SetCellValue("C11", _geometryPageViewModel.Geometry.material_tubes_side);
            doc.SetCellValue("E11", _geometryPageViewModel.Geometry.material_shell_side);
            doc.SetCellValue("D12", _geometryPageViewModel.Geometry.number_of_tubes);
            doc.SetCellValue("D13", _geometryPageViewModel.Geometry.tube_inner_length);
            doc.MergeWorksheetCells("C14", "E14");
            doc.SetCellValue("D14", _geometryPageViewModel.Geometry.tube_inner_length);
            doc.SetCellValue("D15", _geometryPageViewModel.Geometry.wetted_perimeter_tubes_side);
            doc.SetCellValue("E15", _geometryPageViewModel.Geometry.wetted_perimeter_shell_side);
            doc.SetCellValue("D16", _geometryPageViewModel.Geometry.hydraulic_diameter_tubes_side);
            doc.SetCellValue("E16", _geometryPageViewModel.Geometry.hydraulic_diameter_shell_side);
            doc.SetCellValue("D17", _geometryPageViewModel.Geometry.area_module);
            doc.SetCellValue("D18", _geometryPageViewModel.Geometry.volume_module_tubes_side);
            doc.SetCellValue("E18", _geometryPageViewModel.Geometry.volume_module_shell_side);
            doc.MergeWorksheetCells("C19", "D19");
            doc.SetCellValue("C19", _geometryPageViewModel.Geometry.tube_profile_tubes_side);
            doc.SetCellValue("D20", _geometryPageViewModel.Geometry.roughness_tubes_side);
            doc.SetCellValue("E20", _geometryPageViewModel.Geometry.roughness_shell_side);
            doc.MergeWorksheetCells("C21", "E21");
            doc.SetCellValue("C21", _geometryPageViewModel.Geometry.bundle_type);
            doc.SetCellValue("D22", _geometryPageViewModel.Geometry.roller_expanded);
            doc.SetCellValue("D26", _geometryPageViewModel.Geometry.tube_plate_layout_tube_pitch);
            doc.SetCellValue("D27", _geometryPageViewModel.Geometry.tube_plate_layout_tube_layout);
            doc.SetCellValue("D28", _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);
            doc.SetCellValue("D29", _geometryPageViewModel.Geometry.tube_plate_layout_div_plate_layout);
            doc.SetCellValue("D30", _geometryPageViewModel.Geometry.tube_plate_layout_div_plate_thickness);
            doc.SetCellValue("D31", _geometryPageViewModel.Geometry.tube_plate_layout_tubes_cross_section_pre_pass);
            doc.SetCellValue("E31", _geometryPageViewModel.Geometry.tube_plate_layout_shell_cross_section);
            doc.SetCellValue("D32", _geometryPageViewModel.Geometry.tube_plate_layout_perimeter);
            doc.SetCellValue("D33", _geometryPageViewModel.Geometry.tube_plate_layout_max_nr_tubes);
            doc.SetCellValue("D34", _geometryPageViewModel.Geometry.tube_plate_layout_tube_distribution);
            doc.SetCellValue("D35", _geometryPageViewModel.Geometry.tube_plate_layout_tube_tube_spacing);
            doc.SetCellValue("C38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_inner_side);
            doc.SetCellValue("D38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side);
            doc.SetCellValue("E38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side);
            //doc.SetCellValue("C39", _geometryPageViewModel.Geometry.nozzles);
            //doc.SetCellValue("D39", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side);
            //doc.SetCellValue("E39", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side);
            doc.SetCellValue("C40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_inner_side);
            doc.SetCellValue("D40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_tubes_side);
            doc.SetCellValue("E40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_shell_side);
            doc.SetCellValue("D41", _geometryPageViewModel.Geometry.nozzles_in_length_tubes_side);
            doc.SetCellValue("E41", _geometryPageViewModel.Geometry.nozzles_in_length_shell_side);
            doc.SetCellValue("C42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_inner_side);
            doc.SetCellValue("D42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_tubes_side);
            doc.SetCellValue("E42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_shell_side);
            doc.SetCellValue("D44", _geometryPageViewModel.Geometry.nozzles_out_inner_diam_tubes_side);
            doc.SetCellValue("E44", _geometryPageViewModel.Geometry.nozzles_out_inner_diam_shell_side);
            doc.SetCellValue("D45", _geometryPageViewModel.Geometry.nozzles_out_length_tubes_side);
            doc.SetCellValue("E45", _geometryPageViewModel.Geometry.nozzles_out_length_shell_side);
            doc.SetCellValue("D46", _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_tubes_side);
            doc.SetCellValue("E46", _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side);
            doc.SetCellValue("D47", _geometryPageViewModel.Geometry.nozzles_number_of_modules_pre_block);
            doc.SetCellValue("D48", _geometryPageViewModel.Geometry.shell_nozzle_orientation);
            doc.MergeWorksheetCells("C49", "E49");
            doc.SetCellValue("D49", _bafflesPageViewModel.Baffle.number_of_baffles);
            doc.MergeWorksheetCells("C50", "E50");
            doc.SetCellValue("D50", _bafflesPageViewModel.Baffle.buffle_cut);
            doc.MergeWorksheetCells("C51", "E51");
            doc.SetCellValue("D51", _bafflesPageViewModel.Baffle.inlet_baffle_spacing);
            doc.MergeWorksheetCells("C52", "E52");
            doc.SetCellValue("D52", _bafflesPageViewModel.Baffle.central_baffle_spacing);
            doc.MergeWorksheetCells("C53", "E53");
            doc.SetCellValue("D53", _bafflesPageViewModel.Baffle.outlet_baffle_spacing);
            doc.MergeWorksheetCells("C54", "E54");
            doc.SetCellValue("D54", _bafflesPageViewModel.Baffle.baffle_thickness);
            doc.MergeWorksheetCells("C55", "E55");
            doc.SetCellValue("D55", _bafflesPageViewModel.Baffle.pairs_of_sealing_strips);
        }

        private static void AddGeometryUnits(SLDocument doc)
        {
            doc.SetCellValue("B8", "mm");
            doc.SetCellValue("B9", "mm");
            doc.SetCellValue("B10", "mm");
            doc.SetCellValue("B13", "mm");
            doc.SetCellValue("B15", "mm");
            doc.SetCellValue("B16", "mm");
            doc.SetCellValue("B17", "m²");
            doc.SetCellValue("B18", "l");
            doc.SetCellValue("B20", "μm");
            doc.SetCellValue("B26", "mm");
            doc.SetCellValue("B30", "mm");
            doc.SetCellValue("B31", "m²");
            doc.SetCellValue("B32", "mm");
            doc.SetCellValue("B35", "mm");
            doc.SetCellValue("B38", "mm");
            doc.SetCellValue("B39", "mm");
            doc.SetCellValue("B40", "mm");
            doc.SetCellValue("B41", "mm");
            doc.SetCellValue("B42", "mm");
            doc.SetCellValue("B43", "mm");
            doc.SetCellValue("B44", "mm");
            doc.SetCellValue("B45", "mm");
            doc.SetCellValue("B53", "mm");
            doc.SetCellValue("B54", "mm");
            doc.SetCellValue("B55", "mm");
            doc.SetCellValue("B56", "mm");
            doc.SetColumnWidth("B", 15);

        }

        #endregion

        #region overall
        private static void AddOverallData(SLDocument doc)
        {
            doc.AddWorksheet("Heat Transfer Report");
            doc.MergeWorksheetCells("A1", "B1");
            doc.MergeWorksheetCells("A2", "B2");
            doc.MergeWorksheetCells("A3", "B3");
            doc.MergeWorksheetCells("A4", "B4");
            doc.MergeWorksheetCells("C1", "F1");
            doc.MergeWorksheetCells("C3", "F3");
            doc.MergeWorksheetCells("C4", "F4");
            doc.SetCellValue("A1", "Project name");
            doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            doc.SetCellStyle("A1", "A6", BoldTextStyle());
            doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            doc.SetColumnWidth("A", "M", 15);
            doc.SetCellValue("A2", "Revision Nr");
            doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("A3", "Process");
            doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);

            AddGeometryTitle("A6", "F6", "Flow Data", doc);
            AddGeometryTitle("A25", "F25", "Heat Transfer Data", doc);
            AddGeometryTitle("A38", "F38", "Areas", doc);
            AddGeometryTitle("A45", "F45", "Logarithmic Mean Temperature Difference (LMTD)", doc);
            AddGeometryTitle("A50", "F50", "Pressure Drop", doc);
            AddGeometryTitle("A52", "F52", "Tubes Side", doc);
            AddGeometryTitle("A60", "F60", "Shell Side", doc);
            AddGeometryTitle("A70", "F70", "Vibrations", doc);
            doc.MergeWorksheetCells("C7", "D7");
            doc.MergeWorksheetCells("E7", "F7");
            doc.SetCellValue("C7", "Tube Side");
            doc.SetCellValue("E7", "Shell Side");
            doc.SetCellValue("C8", "Inlet");
            doc.SetCellValue("D8", "Outlet");
            doc.SetCellValue("E8", "Inlet");
            doc.SetCellValue("F8", "Outlet");
            doc.SetCellStyle("C7", "F8", BorderCellsStyle());
            doc.SetCellStyle("C7", "F8", BoldTextStyle());
            AddHeatTransferFlowDataNames(doc);
            AddHeatTransferHeatTransferDataNames(doc);
            AddHeatTransferAreaNames(doc);
            AddHeatTransferTempNames(doc);
            AddHeatTransferPressureDropNames(doc);
            AddHeatTransferVibrationsNames(doc);
            AddHeatTransferData(doc);
        }

        private static void AddHeatTransferFlowDataNames(SLDocument doc)
        {
            doc.SetCellValue("A9", "Fluid Name");
            doc.SetCellValue("A10", "Flow");
            doc.SetCellValue("A11", "Temperature");
            doc.SetCellValue("A12", "Duty");
            doc.SetCellValue("A13", "Fluid Velocity");
            doc.SetCellValue("A14", "Shear Rate");
            doc.SetCellValue("A15", "Flow Type");
            doc.SetCellValue("A16", "Liquid Phase");
            doc.SetCellValue("A17", "   App. viscosity");
            doc.SetCellValue("A18", "   Reynolds");
            doc.SetCellValue("A19", "Prandtl");
            doc.SetCellValue("A20", "Gas Phase");
            doc.SetCellValue("A21", "   Dynamic viscosity gas");
            doc.SetCellValue("A22", "   Reynolds");
            doc.SetCellValue("A23", "   Prandtl");
            doc.SetCellStyle("A9", "F23", BorderCellsStyle());
            doc.SetCellStyle("A9", "A23", BoldTextStyle());
        }

        private static void AddHeatTransferHeatTransferDataNames(SLDocument doc)
        {
            doc.SetCellValue("A26", "Wall temperature");
            doc.SetCellValue("A27", "Average metal temp.");
            doc.SetCellValue("A28", "Wall Consistency Index");
            doc.SetCellValue("A29", "Nusselt");
            doc.SetCellValue("A30", "K Side");
            doc.SetCellValue("A31", "Fouling Factor");
            doc.SetCellValue("A33", "K Unfouled");
            doc.SetCellValue("A34", "K Fouled");
            doc.SetCellValue("A35", "K Global Fouled");
            doc.SetCellValue("A36", "K Effective");
            doc.SetCellStyle("A26", "F36", BorderCellsStyle());
            doc.SetCellStyle("A26", "A36", BoldTextStyle());
        }
        private static void AddHeatTransferAreaNames(SLDocument doc)
        {
            doc.SetCellValue("A39", "Surface Area Required");
            doc.SetCellValue("A40", "Area / Module");
            doc.SetCellValue("A41", "Nr. Modules");
            doc.SetCellValue("A42", "Fitted Area");
            doc.SetCellValue("A43", "Excess Area");

            doc.SetCellStyle("A39", "F43", BorderCellsStyle());
            doc.SetCellStyle("A39", "A43", BoldTextStyle());
        }
        private static void AddHeatTransferTempNames(SLDocument doc)
        {
            doc.SetCellValue("A46", "LMTD");
            doc.SetCellValue("A47", "LMTD Correction Factor 'F'");
            doc.SetCellValue("A48", "Adjusted LMTD");

            doc.SetCellStyle("A46", "F48", BorderCellsStyle());
            doc.SetCellStyle("A46", "A48", BoldTextStyle());
        }
        private static void AddHeatTransferPressureDropNames(SLDocument doc)
        {
            doc.SetCellValue("A54", "Modules");
            doc.SetCellValue("A55", "Inlet Nozzles");
            doc.SetCellValue("A56", "Outlet Nozzles");
            doc.SetCellValue("A57", "Bends");
            doc.SetCellValue("A58", "Total");
            doc.SetCellValue("C53", "v  m/s");
            doc.SetCellValue("D53", "ρ·V²  kg/m·s²");
            doc.SetCellValue("E53", "ΔP  bar-a");
            doc.SetCellStyle("C53", "E53", BorderCellsStyle());
            doc.SetCellStyle("C53", "E53", BoldTextStyle());

            doc.SetCellStyle("A54", "E58", BorderCellsStyle());
            doc.SetCellStyle("A54", "A58", BoldTextStyle());

            doc.SetCellValue("A62", "Modules");
            doc.SetCellValue("A63", "Inlet Nozzles");
            doc.SetCellValue("A64", "Outlet Nozzles");
            doc.SetCellValue("A65", "Pressure drop in all central baffle spaces (∆Pc)");
            doc.SetCellValue("A66", "Pressure drop in all baffle windows (∆Pw)");
            doc.SetCellValue("A67", "Pressure drop in the entrance and exit baffle spaces (∆Pe)");
            doc.SetCellValue("A68", "Total");
            doc.SetCellValue("C61", "v  m/s");
            doc.SetCellValue("D61", "ρ·V²  kg/m·s²");
            doc.SetCellValue("E61", "ΔP  bar-a");
            doc.SetCellStyle("C61", "E61", BorderCellsStyle());
            doc.SetCellStyle("C61", "E61", BoldTextStyle());

            doc.SetCellStyle("A62", "E68", BorderCellsStyle());
            doc.SetCellStyle("A62", "A68", BoldTextStyle());
        }
        private static void AddHeatTransferVibrationsNames(SLDocument doc)
        {
            doc.SetCellValue("A72", "Span Length");
            doc.SetCellValue("A73", "Span Length / TEMA Lb,max");
            doc.SetCellValue("A74", "Tubes Natural Frequency");
            doc.SetCellValue("A75", "Shell Acoustic Frequency (gases)");
            doc.SetCellValue("A76", "Fluidelastic Instability");
            doc.SetCellValue("A77", "Cross Flow Velocity");
            doc.SetCellValue("A78", "Critical Velocity");
            doc.SetCellValue("A79", "Average Cross Flow Velocity Ratio");
            doc.SetCellValue("A80", "Vibration Amplitude");
            doc.SetCellValue("A81", "Vortex Shedding Ratio");
            doc.SetCellValue("A82", "Turbulent Buffeting Ratio");
            doc.SetCellValue("A83", "Acoustic Vibrations (gases)");
            doc.SetCellValue("A84", "Acoustic Vibration Exists");
            doc.SetCellValue("A85", "Vibration Exists");
            doc.SetCellStyle("A72", "E85", BorderCellsStyle());
            doc.SetCellStyle("A72", "A85", BoldTextStyle());
        }

        private static void AddHeatTransferData(SLDocument doc)
        {
            doc.MergeWorksheetCells("C9", "D9");
            doc.MergeWorksheetCells("E9", "F9");
            doc.MergeWorksheetCells("C10", "D10");
            doc.MergeWorksheetCells("E10", "F10");
            doc.MergeWorksheetCells("C12", "D12");
            doc.MergeWorksheetCells("E12", "F12");
            doc.MergeWorksheetCells("C27", "D27");
            doc.MergeWorksheetCells("E27", "F27");
            doc.MergeWorksheetCells("C31", "D31");
            doc.MergeWorksheetCells("E31", "F31");
            doc.MergeWorksheetCells("C32", "D32");
            doc.MergeWorksheetCells("E32", "F32");
            doc.MergeWorksheetCells("C33", "D33");
            doc.MergeWorksheetCells("E33", "F33");
            doc.MergeWorksheetCells("C34", "D34");
            doc.MergeWorksheetCells("E34", "F34");
            doc.MergeWorksheetCells("C35", "F35");
            doc.MergeWorksheetCells("C36", "F36");
            doc.MergeWorksheetCells("C39", "F39");
            doc.MergeWorksheetCells("C40", "F40");
            doc.MergeWorksheetCells("C41", "F41");
            doc.MergeWorksheetCells("C42", "F42");
            doc.MergeWorksheetCells("C43", "F43");
            doc.MergeWorksheetCells("C46", "F46");
            doc.MergeWorksheetCells("C47", "F47");
            doc.MergeWorksheetCells("C48", "F48");
            doc.MergeWorksheetCells("A65", "D65");
            doc.MergeWorksheetCells("A66", "D66");
            doc.MergeWorksheetCells("A67", "D67");
            doc.SetCellValue("C9", _overallCalculationViewModel.Overall.fluid_name_tube);
            doc.SetCellValue("E9", _overallCalculationViewModel.Overall.fluid_name_shell);
            doc.SetCellValue("C10", _overallCalculationViewModel.Overall.flow_tube);
            doc.SetCellValue("E10", _overallCalculationViewModel.Overall.flow_shell);
            doc.SetCellValue("C11", _overallCalculationViewModel.Overall.temperature_tube_inlet);
            doc.SetCellValue("D11", _overallCalculationViewModel.Overall.temperature_tube_outlet);
            doc.SetCellValue("E11", _overallCalculationViewModel.Overall.temperature_shell_inlet);
            doc.SetCellValue("F11", _overallCalculationViewModel.Overall.temperature_shell_outlet);
            doc.SetCellValue("C12", _overallCalculationViewModel.Overall.duty_tube);
            doc.SetCellValue("E12", _overallCalculationViewModel.Overall.duty_shell);
            doc.SetCellValue("C13", _overallCalculationViewModel.Overall.fluid_velocity_tube_inlet);
            doc.SetCellValue("D13", _overallCalculationViewModel.Overall.fluid_velocity_tube_outlet);
            doc.SetCellValue("E13", _overallCalculationViewModel.Overall.fluid_velocity_shell_inlet);
            doc.SetCellValue("F13", _overallCalculationViewModel.Overall.fluid_velocity_shell_outlet);
            doc.SetCellValue("C14", _overallCalculationViewModel.Overall.shear_rate_tube_inlet);
            doc.SetCellValue("D14", _overallCalculationViewModel.Overall.shear_rate_tube_outlet);
            doc.SetCellValue("E14", _overallCalculationViewModel.Overall.shear_rate_shell_inlet);
            doc.SetCellValue("F14", _overallCalculationViewModel.Overall.shear_rate_shell_outlet);
            doc.SetCellValue("C15", _overallCalculationViewModel.Overall.flow_type_tube_inlet);
            doc.SetCellValue("D15", _overallCalculationViewModel.Overall.flow_type_tube_outlet);
            doc.SetCellValue("E15", _overallCalculationViewModel.Overall.flow_type_shell_inlet);
            doc.SetCellValue("F15", _overallCalculationViewModel.Overall.flow_type_shell_outlet);

            doc.SetCellValue("C17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_inlet);
            doc.SetCellValue("D17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_outlet);
            doc.SetCellValue("E17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet);
            doc.SetCellValue("F17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet);
            doc.SetCellValue("C18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_tube_inlet);
            doc.SetCellValue("D18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_tube_outlet);
            doc.SetCellValue("E18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_shell_inlet);
            doc.SetCellValue("F18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_shell_outlet);
            doc.SetCellValue("C19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_tube_inlet);
            doc.SetCellValue("D19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_tube_outlet);
            doc.SetCellValue("E19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_shell_inlet);
            doc.SetCellValue("F19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_shell_outlet);

            doc.SetCellValue("C21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_tube_inlet);
            doc.SetCellValue("D21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_tube_outlet);
            doc.SetCellValue("E21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_shell_inlet);
            doc.SetCellValue("F21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_shell_outlet);
            doc.SetCellValue("C22", _overallCalculationViewModel.Overall.gas_phase_reynolds_tube_inlet);
            doc.SetCellValue("D22", _overallCalculationViewModel.Overall.gas_phase_reynolds_tube_outlet);
            doc.SetCellValue("E22", _overallCalculationViewModel.Overall.gas_phase_reynolds_shell_inlet);
            doc.SetCellValue("F22", _overallCalculationViewModel.Overall.gas_phase_reynolds_shell_outlet);
            doc.SetCellValue("C23", _overallCalculationViewModel.Overall.gas_phase_prandtl_tube_inlet);
            doc.SetCellValue("D23", _overallCalculationViewModel.Overall.gas_phase_prandtl_tube_outlet);
            doc.SetCellValue("E23", _overallCalculationViewModel.Overall.gas_phase_prandtl_shell_inlet);
            doc.SetCellValue("F23", _overallCalculationViewModel.Overall.gas_phase_prandtl_shell_outlet);

            doc.SetCellValue("C26", _overallCalculationViewModel.Overall.wall_temperature_tube_inlet);
            doc.SetCellValue("D26", _overallCalculationViewModel.Overall.wall_temperature_tube_outlet);
            doc.SetCellValue("E26", _overallCalculationViewModel.Overall.wall_temperature_shell_inlet);
            doc.SetCellValue("F26", _overallCalculationViewModel.Overall.wall_temperature_shell_outlet);

            //TODO: Добавить свойство average metall temp

            doc.SetCellValue("C28", _overallCalculationViewModel.Overall.wall_consistency_index_tube_inlet);
            doc.SetCellValue("D28", _overallCalculationViewModel.Overall.wall_consistency_index_tube_outlet);
            doc.SetCellValue("E28", _overallCalculationViewModel.Overall.wall_consistency_index_shell_inlet);
            doc.SetCellValue("F28", _overallCalculationViewModel.Overall.wall_consistency_index_shell_inlet);
            doc.SetCellValue("C29", _overallCalculationViewModel.Overall.nusselt_tube_inlet);
            doc.SetCellValue("D29", _overallCalculationViewModel.Overall.nusselt_tube_outlet);
            doc.SetCellValue("E29", _overallCalculationViewModel.Overall.nusselt_shell_inlet);
            doc.SetCellValue("F29", _overallCalculationViewModel.Overall.nusselt_shell_outlet);
            doc.SetCellValue("C30", _overallCalculationViewModel.Overall.k_side_tube_inlet);
            doc.SetCellValue("D30", _overallCalculationViewModel.Overall.k_side_tube_outlet);
            doc.SetCellValue("E30", _overallCalculationViewModel.Overall.k_side_shell_inlet);
            doc.SetCellValue("F30", _overallCalculationViewModel.Overall.k_side_shell_outlet);
            doc.SetCellValue("C31", _overallCalculationViewModel.Overall.fouling_factor_tube);
            doc.SetCellValue("E31", _overallCalculationViewModel.Overall.fouling_factor_shell);
            doc.SetCellValue("C32", "Inlet");
            doc.SetCellValue("E32", "Outlet");
            doc.SetCellValue("C33", _overallCalculationViewModel.Overall.k_unfouled_inlet);
            doc.SetCellValue("E33", _overallCalculationViewModel.Overall.k_unfouled_outlet);
            doc.SetCellValue("C34", _overallCalculationViewModel.Overall.k_fouled_inlet);
            doc.SetCellValue("E34", _overallCalculationViewModel.Overall.k_fouled_outlet);
            doc.SetCellValue("C35", _overallCalculationViewModel.Overall.k_global_fouled);
            doc.SetCellValue("C36", _overallCalculationViewModel.Overall.k_effective);
            doc.SetCellValue("C39", _overallCalculationViewModel.Overall.surface_area_required);
            doc.SetCellValue("C40", _overallCalculationViewModel.Overall.area_module);
            doc.SetCellValue("C41", _overallCalculationViewModel.Overall.nr_modules);
            doc.SetCellValue("C42", _overallCalculationViewModel.Overall.area_fitted);
            doc.SetCellValue("C43", _overallCalculationViewModel.Overall.excess_area);
            doc.SetCellValue("C46", _overallCalculationViewModel.Overall.LMTD);
            doc.SetCellValue("C47", _overallCalculationViewModel.Overall.LMTD_correction_factor);
            doc.SetCellValue("C48", _overallCalculationViewModel.Overall.adjusted_LMTD);
            doc.SetCellValue("C54", _overallCalculationViewModel.Overall.pressure_drop_tube_side_modules_V);
            doc.SetCellValue("E54", _overallCalculationViewModel.Overall.pressure_drop_tube_side_modules_P);
            doc.SetCellValue("C55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_V);
            doc.SetCellValue("D55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_pV);
            doc.SetCellValue("E55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_P);
            doc.SetCellValue("C56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_V);
            doc.SetCellValue("D56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_pV);
            doc.SetCellValue("E56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_P);
            doc.SetCellValue("C57", _overallCalculationViewModel.Overall.pressure_drop_tube_side_bends_V);
            doc.SetCellValue("E57", _overallCalculationViewModel.Overall.pressure_drop_tube_side_bends_P);
            doc.SetCellValue("E58", _overallCalculationViewModel.Overall.pressure_drop_tube_side_total_P);
            doc.SetCellValue("C62", _overallCalculationViewModel.Overall.pressure_drop_shell_side_modules_V);
            doc.SetCellValue("E62", _overallCalculationViewModel.Overall.pressure_drop_shell_side_modules_P);
            doc.SetCellValue("C63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_V);
            doc.SetCellValue("D63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_pV);
            doc.SetCellValue("E63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_P);

            doc.SetCellValue("C64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_V);
            doc.SetCellValue("D64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_pV);
            doc.SetCellValue("E64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_P);
            //doc.SetCellValue("E65", _overallCalculationViewModel.Overall.win);
            doc.SetCellValue("E68", _overallCalculationViewModel.Overall.pressure_drop_shell_side_total_p);
            doc.SetCellValue("C72", _overallCalculationViewModel.Overall.vibrations_inlet_span_length);
            doc.SetCellValue("D72", _overallCalculationViewModel.Overall.vibrations_central_span_length);
            doc.SetCellValue("E72", _overallCalculationViewModel.Overall.vibrations_outlet_span_length);
            doc.SetCellValue("C73", _overallCalculationViewModel.Overall.vibrations_inlet_span_length_tema_lb);
            doc.SetCellValue("D73", _overallCalculationViewModel.Overall.vibrations_central_span_length_tema_lb);
            doc.SetCellValue("E73", _overallCalculationViewModel.Overall.vibrations_outlet_span_length_tema_lb);
            doc.SetCellValue("C74", _overallCalculationViewModel.Overall.vibrations_inlet_tubes_natural_frequency);
            doc.SetCellValue("D74", _overallCalculationViewModel.Overall.vibrations_central_tubes_natural_frequency);
            doc.SetCellValue("E74", _overallCalculationViewModel.Overall.vibrations_outlet_tubes_natural_frequency);
            doc.SetCellValue("C75", _overallCalculationViewModel.Overall.vibrations_inlet_shell_acoustic_frequency_gases);
            doc.SetCellValue("D75", _overallCalculationViewModel.Overall.vibrations_central_shell_acoustic_frequency_gases);
            doc.SetCellValue("E75", _overallCalculationViewModel.Overall.vibrations_outlet_shell_acoustic_frequency_gases);

            doc.SetCellValue("C77", _overallCalculationViewModel.Overall.vibrations_inlet_cross_flow_velocity);
            doc.SetCellValue("D77", _overallCalculationViewModel.Overall.vibrations_central_cross_flow_velocity);
            doc.SetCellValue("E77", _overallCalculationViewModel.Overall.vibrations_outlet_cross_flow_velocity);
            doc.SetCellValue("C78", _overallCalculationViewModel.Overall.vibrations_inlet_cricical_velocity);
            doc.SetCellValue("D78", _overallCalculationViewModel.Overall.vibrations_central_cricical_velocity);
            doc.SetCellValue("E78", _overallCalculationViewModel.Overall.vibrations_outlet_cricical_velocity);
            doc.SetCellValue("C79", _overallCalculationViewModel.Overall.vibrations_inlet_average_cross_flow_velocity_ratio);
            doc.SetCellValue("D79", _overallCalculationViewModel.Overall.vibrations_central_average_cross_flow_velocity_ratio);
            doc.SetCellValue("E79", _overallCalculationViewModel.Overall.vibrations_outlet_average_cross_flow_velocity_ratio);

            doc.SetCellValue("C81", _overallCalculationViewModel.Overall.vibrations_inlet_vortex_shedding_ratio);
            doc.SetCellValue("D81", _overallCalculationViewModel.Overall.vibrations_central_vortex_shedding_ratio);
            doc.SetCellValue("E81", _overallCalculationViewModel.Overall.vibrations_outlet_vortex_shedding_ratio);
            doc.SetCellValue("C82", _overallCalculationViewModel.Overall.vibrations_inlet_turbulent_buffeting_ratio);
            doc.SetCellValue("D82", _overallCalculationViewModel.Overall.vibrations_central_turbulent_buffeting_ratio);
            doc.SetCellValue("E82", _overallCalculationViewModel.Overall.vibrations_outlet_turbulent_buffeting_ratio);

            doc.SetCellValue("C84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_inlet == 1 ? "Yes" : "No");
            doc.SetCellValue("D84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_central == 1 ? "Yes" : "No");
            doc.SetCellValue("E84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_outlet == 1 ? "Yes" : "No");

        }
            
        #endregion

    }
}
