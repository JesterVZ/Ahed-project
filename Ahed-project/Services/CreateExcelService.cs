using Ahed_project.MasterData;
using Ahed_project.MasterData.Graphs;
using Ahed_project.MasterData.Products;
using Ahed_project.Services.Global;
using Ahed_project.Settings;
using Ahed_project.Utils;
using Ahed_project.ViewModel.Pages;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using SpreadsheetLight;
using SpreadsheetLight.Charts;
using SpreadsheetLight.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private static GraphsPageViewModel _graphsPageViewModel;
        public CreateExcelService(
            TubesFluidViewModel tubesFluidViewModel,
            ShellFluidViewModel shellFluidViewModel,
            ProjectPageViewModel projectPageViewModel,
            HeatBalanceViewModel heatBalanceViewModel,
            GeometryPageViewModel geometryPageViewModel,
            BafflesPageViewModel bafflesPageViewModel,
            OverallCalculationViewModel overallCalculationViewModel,
            GraphsPageViewModel graphsPageViewModel)
        {
            _projectPageViewModel = projectPageViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;
            _geometryPageViewModel = geometryPageViewModel;
            _bafflesPageViewModel = bafflesPageViewModel;
            _overallCalculationViewModel = overallCalculationViewModel;
            _graphsPageViewModel = graphsPageViewModel;
        }
        public void CreateExcel()
        {
            CreateFull();
        }

        private void CreateFull()
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
                    AddBafflesData(doc);
                    AddOverallData(doc);
                    AddGraphsData(doc);
                    AddTemaSheet(doc);
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

        private void AddBafflesData(SLDocument doc)
        {
            doc.AddWorksheet("BafflesReport");
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
            doc.SetCellValue("A4", "Module name");
            doc.SetCellValue("C4", _overallCalculationViewModel.Name);

            AddBaffleNames(doc);
            AddBafflesUom(doc);
            AddBafflesUomData(doc);
        }

        private void AddBafflesUomData(SLDocument doc)
        {
            doc.SetCellValue("C7", _bafflesPageViewModel.Baffle.type.ToNormalCase());
            doc.SetCellValue("C8", _bafflesPageViewModel.Baffle.shell_inner_diameter.ToDoubleWithoutDot());
            doc.SetCellValue("C9", _bafflesPageViewModel.Baffle.tubes_outer_diameter.ToDoubleWithoutDot());
            doc.SetCellValue("C10", _bafflesPageViewModel.Baffle.buffle_cut.ToDoubleWithoutDot());
            doc.SetCellValue("C11", _bafflesPageViewModel.Baffle.buffle_cut_diraction.ToNormalCase());
            doc.SetCellValue("C12", _bafflesPageViewModel.Baffle.pairs_of_sealing_strips.ToDoubleWithoutDot());
            doc.SetCellValue("C13", _bafflesPageViewModel.Baffle.shell_diameter_angle.ToDoubleWithoutDot());
            doc.SetCellValue("C14", _bafflesPageViewModel.Baffle.center_tube_angle.ToDoubleWithoutDot());
            doc.SetCellValue("C15", _bafflesPageViewModel.Baffle.diameter_to_tube_center.ToDoubleWithoutDot());
            doc.SetCellValue("C16", _bafflesPageViewModel.Baffle.diameter_to_tube_outer_side.ToDoubleWithoutDot());
            doc.SetCellValue("C17", _bafflesPageViewModel.Baffle.bypass_lanes);
            doc.SetCellValue("C18", _bafflesPageViewModel.Baffle.inner_shell_to_outer_tube_bypass_clearance.ToDoubleWithoutDot());
            doc.SetCellValue("C19", _bafflesPageViewModel.Baffle.average_tubes_in_baffle_windows.ToDoubleWithoutDot());

            doc.SetCellValue("C22", _bafflesPageViewModel.Baffle.diametral_clearance_shell_baffle.ToDoubleWithoutDot());
            doc.SetCellValue("C23", _bafflesPageViewModel.Baffle.diametral_clearance_tube_baffle.ToDoubleWithoutDot());

            doc.SetCellValue("C26", _bafflesPageViewModel.Baffle.tubeplate_thickness.ToDoubleWithoutDot());
            doc.SetCellValue("C27", _bafflesPageViewModel.Baffle.tube_inner_length.ToDoubleWithoutDot());
            doc.SetCellValue("C28", _bafflesPageViewModel.Baffle.tube_outer_length.ToDoubleWithoutDot());
            doc.SetCellValue("C29", _bafflesPageViewModel.Baffle.inlet_baffle_spacing.ToDoubleWithoutDot());
            doc.SetCellValue("C30", _bafflesPageViewModel.Baffle.central_baffle_spacing.ToDoubleWithoutDot());
            doc.SetCellValue("C31", _bafflesPageViewModel.Baffle.outlet_baffle_spacing.ToDoubleWithoutDot());
            doc.SetCellValue("C32", _bafflesPageViewModel.Baffle.number_of_baffles);
            doc.SetCellValue("C33", _bafflesPageViewModel.Baffle.baffle_thickness.ToDoubleWithoutDot());

            doc.SetCellValue("C37", _bafflesPageViewModel.Baffle.cut_effect_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D37", _bafflesPageViewModel.Baffle.cut_effect_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C38", _bafflesPageViewModel.Baffle.leackages_effect_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D38", _bafflesPageViewModel.Baffle.leackages_effect_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C39", _bafflesPageViewModel.Baffle.bundle_bypass_effect_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D39", _bafflesPageViewModel.Baffle.bundle_bypass_effect_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C40", _bafflesPageViewModel.Baffle.adverce_temperature_gradient_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D40", _bafflesPageViewModel.Baffle.adverce_temperature_gradient_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C41", _bafflesPageViewModel.Baffle.uneven_baffle_spacing_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D41", _bafflesPageViewModel.Baffle.uneven_baffle_spacing_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C42", _bafflesPageViewModel.Baffle.combined_effects_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D42", _bafflesPageViewModel.Baffle.combined_effects_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C44", _bafflesPageViewModel.Baffle.colorbun_correction_factor_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D44", _bafflesPageViewModel.Baffle.colorbun_correction_factor_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C45", _bafflesPageViewModel.Baffle.heat_trans_coeff_pure_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D45", _bafflesPageViewModel.Baffle.heat_trans_coeff_pure_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C46", _bafflesPageViewModel.Baffle.shell_side_heat_transfer_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D46", _bafflesPageViewModel.Baffle.shell_side_heat_transfer_outlet.ToDoubleWithoutDot());
        }

        private void AddBafflesUom(SLDocument doc)
        {
            doc.SetCellValue("B8", "mm");
            doc.SetCellValue("B9", "mm");
            doc.SetCellValue("B10", "%");
            doc.SetCellValue("B13", "°");
            doc.SetCellValue("B14", "°");
            doc.SetCellValue("B15", "mm");
            doc.SetCellValue("B16", "mm");
            doc.SetCellValue("B17", "mm");
            doc.SetCellValue("B18", "mm");

            doc.SetCellValue("B22", "mm");
            doc.SetCellValue("B23", "mm");

            doc.SetCellValue("B26", "mm");
            doc.SetCellValue("B27", "mm");
            doc.SetCellValue("B28", "mm");
            doc.SetCellValue("B29", "mm");
            doc.SetCellValue("B30", "mm");
            doc.SetCellValue("B31", "mm");
            doc.SetCellValue("B33", "mm");

            doc.SetCellValue("B45", "W/m²·°C");
            doc.SetCellValue("B46", "W/m²·°C");
        }

        private void AddBaffleNames(SLDocument doc)
        {
            doc.MergeWorksheetCells("A6", "D6");
            doc.SetCellStyle("A6", BoldHeaderTextStyle());
            doc.SetCellValue("A6", "Baffle Geometry Parameters");

            doc.SetCellStyle("A7", "D19", BorderCellsStyle());
            doc.SetCellStyle("A7", "A19", BoldTextStyle());

            doc.SetCellValue("A7", "Type");
            doc.SetCellValue("A8", "Shell inner diameter (Ds)");
            doc.SetCellValue("A9", "Tubes Outer Diameter (Dt)");
            doc.SetCellValue("A10", "Baffle cut  (Bc)");
            doc.SetCellValue("A11", "Baffle cut direction");
            doc.SetCellValue("A12", "Pairs of sealing strips (Nss)");
            doc.SetCellValue("A13", "Shell diameter angle (θds)");
            doc.SetCellValue("A14", "Center tube angle (θctℓ)");
            doc.SetCellValue("A15", "Diameter to tube center (Dctℓ)");
            doc.SetCellValue("A16", "Diameter to tube outer side (Dotℓ)");
            doc.SetCellValue("A17", "Bypass lanes (Lp)");
            doc.SetCellValue("A18", "Inner shell to outer tube bypass clearance (Lbb)");
            doc.SetCellValue("A19", "Average Tubes in Baffle Windows");

            doc.MergeWorksheetCells("A21", "D21");
            doc.SetCellStyle("A21", BoldHeaderTextStyle());
            doc.SetCellValue("A21", "Baffles Clearances & Spacings");

            doc.SetCellStyle("A22", "D23", BorderCellsStyle());
            doc.SetCellStyle("A22", "A23", BoldTextStyle());

            doc.SetCellValue("A22", "Diametral Clearance Shell-Baffle (δsb)");
            doc.SetCellValue("A23", "Diametral Clearance Tube-Baffle (δtb)");

            doc.MergeWorksheetCells("A25", "D25");
            doc.SetCellStyle("A25", BoldHeaderTextStyle());
            doc.SetCellValue("A25", "Baffle Distribution Parameters");

            doc.SetCellStyle("A26", "D33", BorderCellsStyle());
            doc.SetCellStyle("A26", "A33", BoldTextStyle());

            doc.SetCellValue("A26", "Tubeplate Thickness (Lts)");
            doc.SetCellValue("A27", "Tube Inner Length (Lti)");
            doc.SetCellValue("A28", "Tube Outer Length (Lto)");
            doc.SetCellValue("A29", "Inlet baffle spacing (Lbi)");
            doc.SetCellValue("A30", "Central baffle spacing (Lbc)");
            doc.SetCellValue("A31", "Outlet baffle spacing (Lbo)");
            doc.SetCellValue("A32", "Number of baffles (Nb)");
            doc.SetCellValue("A33", "Baffle thickness (Bt)");

            doc.MergeWorksheetCells("A35", "D35");
            doc.SetCellStyle("A35", BoldHeaderTextStyle());
            doc.SetCellValue("A35", "Baffle Efects and Factors");

            doc.SetCellStyle("A37", "D46", BorderCellsStyle());
            doc.SetCellStyle("A37", "A46", BoldTextStyle());

            doc.SetCellValue("A37", "Cut Effect (Jc)");
            doc.SetCellValue("A38", "Leakages Effect (Jl)");
            doc.SetCellValue("A39", "Bundle bypass effect (Jb)");
            doc.SetCellValue("A40", "Adverse Temperature Gradient (Jr)");
            doc.SetCellValue("A41", "Uneven Baffle Spacing (Js)");
            doc.SetCellValue("A42", "Combined Effects (JT)");
            doc.SetCellValue("A44", "Colburn Correction Factor (Ji)");
            doc.SetCellValue("A45", "Heat Trans. Coeff. Pure Cross-flow Ideal (Hccid)");
            doc.SetCellValue("A46", "Shell-Side Heat Transfer Coefficient (Hcc)");

            doc.SetCellStyle("C36", "D36", BorderCellsStyle());
            doc.SetCellStyle("C36", "D36", BoldTextStyle());

            doc.SetCellValue("C36", "Inlet");
            doc.SetCellValue("D36", "Outlet");
        }

        #region TemaSheet
        private void AddTemaSheet(SLDocument doc)
        {
            var ass = Assembly.GetExecutingAssembly();
            doc.AddWorksheet("TEMA Sheet");
            doc.SetCellStyle("A1", "Q62", BorderCellsStyle(true));
            doc.MergeWorksheetCells("A1", "Q1");
            doc.SetCellValue("A1", "Customer and revision information");
            doc.SetCellStyle("A1", TemaHeaderStyle());
            doc.SetCellValue("A2", "1");
            doc.SetCellValue("A3", "2");
            doc.SetCellValue("A4", "3");
            doc.SetCellValue("A5", "4");
            doc.MergeWorksheetCells("B2", "C2");
            doc.MergeWorksheetCells("B3", "C3");
            doc.MergeWorksheetCells("B4", "C4");
            doc.MergeWorksheetCells("B5", "C5");
            doc.SetCellValue("B2", "Customer");
            doc.SetCellValue("B3", "Revision Nr");
            doc.SetCellValue("B4", "Date");
            doc.SetCellValue("B5", "Author");
            doc.MergeWorksheetCells("D2", "K2");
            doc.MergeWorksheetCells("D3", "K3");
            doc.MergeWorksheetCells("D4", "K4");
            doc.MergeWorksheetCells("D5", "K5");
            doc.SetCellValue("D2", GlobalDataCollectorService.User.name);
            doc.SetCellValue("D3", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("D4", DateTime.Now.ToString("dddd, MMMM dd, dddd (HH:mm:ss)"));
            doc.SetCellValue("D5", GlobalDataCollectorService.User.email);
            doc.MergeWorksheetCells("L2", "Q6");
            var picture = new SLPicture($"{System.IO.Path.GetDirectoryName(ass.Location)}\\Visual\\header.png");
            picture.SetPosition(1, 11);
            picture.ResizeInPixels(383, 100);
            doc.InsertPicture(picture);
            doc.MergeWorksheetCells("A6", "K6");
            doc.SetCellStyle("A6", TemaHeaderStyle());
            doc.SetCellValue("A6", "Heat exchanger summary");
            doc.SetCellStyle("C7", "Q8", BorderCellsStyle());
            doc.SetCellValue("A7", "5");
            doc.SetCellValue("A8", "6");
            doc.SetCellValue("B7", "Module");
            doc.SetCellValue("B8", "Area / Module");
            doc.SetCellStyle("C7", "Q8", BorderCellsStyle(true));
            doc.MergeWorksheetCells("C7", "D7");
            doc.SetCellValue("C7", _overallCalculationViewModel.Name);
            doc.SetCellValue("C8", "m²");
            doc.SetCellValue("D8", _overallCalculationViewModel.Overall.area_module.ToDoubleWithoutDot());
            doc.SetCellValue("E7", "Nr.");
            doc.SetCellValue("E8", "Total area");
            doc.MergeWorksheetCells("F7", "H7");
            doc.MergeWorksheetCells("F8", "H8");
            doc.SetCellValue("F7", _overallCalculationViewModel.Overall.nr_modules);
            doc.SetCellValue("F8", "m²");
            doc.MergeWorksheetCells("I7", "N7");
            doc.MergeWorksheetCells("I8", "Q8");
            doc.SetCellValue("I7", "Parallel Lines (Tubes/Shell)");
            doc.SetCellValue("I8", _overallCalculationViewModel.Overall.excess_area.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("O7", "Q7");
            doc.SetCellValue("O7", $"{_overallCalculationViewModel.Overall.nozzles_number_of_parallel_lines_tube_side ?? _overallCalculationViewModel.Overall.nozzles_number_of_parallel_lines_shell_side}/{_overallCalculationViewModel.Overall.nozzles_number_of_parallel_lines_shell_side}");
            doc.MergeWorksheetCells("A9", "Q9");
            doc.SetCellStyle("A9", TemaHeaderStyle());
            doc.SetCellValue("A9", "Heat exchanger performance");
            for (int i = 7; i <= 26; i++)
            {
                doc.SetCellValue($"A{i + 3}", $"{i}");
                doc.MergeWorksheetCells($"B{i + 3}", $"D{i + 3}");
            }
            doc.SetCellValue("B11", "Fluid Name");
            doc.SetCellValue("B12", "Flow total");
            doc.SetCellValue("B13", "Gas phase");
            doc.SetCellValue("B14", "Liquid phase");
            doc.SetCellValue("B15", "Temperature");
            doc.SetCellValue("B16", "Density");
            doc.SetCellValue("B17", "Specific heat");
            doc.SetCellValue("B18", "Therm. Cond.");
            doc.SetCellValue("B19", "Viscosity");
            doc.SetCellValue("B20", "Latent heat");
            doc.SetCellValue("B21", "Vapour pressure");
            doc.SetCellValue("B22", "Velocity");
            doc.SetCellValue("B23", "Max pressure drop");
            doc.SetCellValue("B24", "Calculated pressure drop");
            doc.SetCellValue("B25", "LMTD corrected");
            doc.SetCellValue("B26", "Duty");
            doc.SetCellValue("B27", "Global fouled coefficient");
            doc.SetCellValue("B28", "Fouling factor");
            doc.SetCellValue("B29", "Effective heat transfer coefficient");

            doc.SetCellValue("E12", "kg/hr");
            doc.SetCellValue("E13", "kg/hr");
            doc.SetCellValue("E14", "kg/hr");
            doc.SetCellValue("E15", "°C");
            doc.SetCellValue("E16", "kg/m³");
            doc.SetCellValue("E17", "kJ/kg·°C");
            doc.SetCellValue("E18", "W/m·°C");
            doc.SetCellValue("E19", "cP");
            doc.SetCellValue("E20", "J/kg");
            doc.SetCellValue("E21", "bar-a");
            doc.SetCellValue("E22", "m/s");
            doc.SetCellValue("E23", "bar-a");
            doc.SetCellValue("E24", "bar-a");
            doc.SetCellValue("E25", "°C");
            doc.SetCellValue("E26", "kW");
            doc.SetCellValue("E27", "W/m²·°C");
            doc.SetCellValue("E28", "m²·°C/W");
            doc.SetCellValue("E29", "W/m²·°C");

            doc.MergeWorksheetCells("F10", "H10");
            doc.SetCellValue("F10", "Tube In");
            doc.SetCellStyle("F10", BoldCenteredBordered());

            doc.MergeWorksheetCells("I10", "K10");
            doc.SetCellValue("I10", "Tube Out");
            doc.SetCellStyle("I10", BoldCenteredBordered());

            doc.MergeWorksheetCells("L10", "N10");
            doc.SetCellValue("L10", "Shell In");
            doc.SetCellStyle("L10", BoldCenteredBordered());

            doc.MergeWorksheetCells("O10", "Q10");
            doc.SetCellValue("O10", "Shell Out");
            doc.SetCellStyle("O10", BoldCenteredBordered());

            //Flow total
            doc.MergeWorksheetCells("F11", "K11");
            doc.SetCellValue("F11", _tubesFluidViewModel.Product.name);

            doc.MergeWorksheetCells("L11", "Q11");
            doc.SetCellValue("L11", _shellFluidViewModel.Product.name);

            doc.MergeWorksheetCells("F12", "K12");
            doc.SetCellValue("F12", _overallCalculationViewModel.Overall.flow_tube.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L12", "Q12");
            doc.SetCellValue("L12", _overallCalculationViewModel.Overall.flow_shell.ToDoubleWithoutDot());

            //Gas phase
            doc.MergeWorksheetCells("F13", "H13");
            doc.MergeWorksheetCells("I13", "K13");
            doc.MergeWorksheetCells("L13", "N13");
            doc.MergeWorksheetCells("O13", "Q13");

            //Liquid phase
            doc.MergeWorksheetCells("F14", "H14");
            doc.SetCellValue("F14", _heatBalanceViewModel.Calculation.flow_tube.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I14", "K14");
            doc.SetCellValue("I14", _heatBalanceViewModel.Calculation.flow_tube.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L14", "N14");
            doc.SetCellValue("L14", _heatBalanceViewModel.Calculation.flow_shell.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O14", "Q14");
            doc.SetCellValue("O14", _heatBalanceViewModel.Calculation.flow_shell.ToDoubleWithoutDot());

            //Temperature
            doc.MergeWorksheetCells("F15", "H15");
            doc.SetCellValue("F15", _overallCalculationViewModel.Overall.temperature_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I15", "K15");
            doc.SetCellValue("I15", _overallCalculationViewModel.Overall.temperature_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L15", "N15");
            doc.SetCellValue("L15", _overallCalculationViewModel.Overall.temperature_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O15", "Q15");
            doc.SetCellValue("O15", _overallCalculationViewModel.Overall.temperature_shell_outlet.ToDoubleWithoutDot());

            //Density
            doc.MergeWorksheetCells("F16", "H16");
            doc.SetCellValue("F16", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I16", "K16");
            doc.SetCellValue("I16", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L16", "N16");
            doc.SetCellValue("L16", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O16", "Q16");
            doc.SetCellValue("O16", _heatBalanceViewModel.Calculation.liquid_density_shell_outlet.ToDoubleWithoutDot());

            //Specific Heat
            doc.MergeWorksheetCells("F17", "H17");
            doc.SetCellValue("F17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I17", "K17");
            doc.SetCellValue("I17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L17", "N17");
            doc.SetCellValue("L17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O17", "Q17");
            doc.SetCellValue("O17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_outlet.ToDoubleWithoutDot());

            //Therm. Cond.
            doc.MergeWorksheetCells("F18", "H18");
            doc.SetCellValue("F18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I18", "K18");
            doc.SetCellValue("I18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L18", "N18");
            doc.SetCellValue("L18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O18", "Q18");
            doc.SetCellValue("O18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_outlet.ToDoubleWithoutDot());

            //Viscosity
            doc.MergeWorksheetCells("F19", "H19");
            doc.SetCellValue("F19", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I19", "K19");
            doc.SetCellValue("I19", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L19", "N19");
            doc.SetCellValue("L19", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O19", "Q19");
            doc.SetCellValue("O19", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_outlet.ToDoubleWithoutDot());

            //Latent heat
            doc.MergeWorksheetCells("F20", "H20");
            doc.SetCellValue("F20", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I20", "K20");
            doc.SetCellValue("I20", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L20", "N20");
            doc.SetCellValue("L20", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O20", "Q20");
            doc.SetCellValue("O20", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_outlet.ToDoubleWithoutDot());

            //Vapour pressure
            doc.MergeWorksheetCells("F21", "H21");
            doc.SetCellValue("F21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I21", "K21");
            doc.SetCellValue("I21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L21", "N21");
            doc.SetCellValue("L21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O21", "Q21");
            doc.SetCellValue("O21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_outlet.ToDoubleWithoutDot());

            //Velocity
            doc.MergeWorksheetCells("F22", "H22");
            doc.SetCellValue("F22", _overallCalculationViewModel.Overall.fluid_velocity_tube_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("I22", "K22");
            doc.SetCellValue("I22", _overallCalculationViewModel.Overall.fluid_velocity_tube_outlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("L22", "N22");
            doc.SetCellValue("L22", _overallCalculationViewModel.Overall.fluid_velocity_shell_inlet.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("O22", "Q22");
            doc.SetCellValue("O22", _overallCalculationViewModel.Overall.fluid_velocity_shell_outlet.ToDoubleWithoutDot());

            //Max pressure drop
            //doc.MergeWorksheetCells("F23", "H23");
            //doc.SetCellValue("F23", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_P);

            //doc.MergeWorksheetCells("I23", "K23");
            //doc.SetCellValue("I23", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_P);

            //doc.MergeWorksheetCells("L23", "N23");
            //doc.SetCellValue("L23", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_P);

            //doc.MergeWorksheetCells("O23", "Q23");
            //doc.SetCellValue("O23", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_P);

            //Calculated pressure drop
            //doc.MergeWorksheetCells("F24", "H24");
            //doc.SetCellValue("F24", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_pV);

            //doc.MergeWorksheetCells("I24", "K24");
            //doc.SetCellValue("I24", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_pV);

            //doc.MergeWorksheetCells("L24", "N24");
            //doc.SetCellValue("L24", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_pV);

            //doc.MergeWorksheetCells("O24", "Q24");
            //doc.SetCellValue("O24", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_pV);

            //LMTD corrected
            doc.MergeWorksheetCells("F25", "Q25");
            doc.SetCellValue("F25", _overallCalculationViewModel.Overall.LMTD.ToDoubleWithoutDot());

            //Duty
            doc.MergeWorksheetCells("F26", "Q26");
            doc.SetCellValue("F26", (StringToDoubleChecker.ConvertToDouble(_overallCalculationViewModel.Overall.duty_tube) + StringToDoubleChecker.ConvertToDouble(_overallCalculationViewModel.Overall.duty_shell)).ToString("F"));

            //Global fouled coefficient
            doc.MergeWorksheetCells("F27", "Q27");
            doc.SetCellValue("F27", _overallCalculationViewModel.Overall.k_global_fouled.ToDoubleWithoutDot());

            //Fouling factor
            doc.MergeWorksheetCells("F28", "K28");
            doc.SetCellValue("F28", _overallCalculationViewModel.Overall.fouling_factor_tube);

            doc.MergeWorksheetCells("L28", "Q28");
            doc.SetCellValue("L28", _overallCalculationViewModel.Overall.fouling_factor_shell);

            //Effective heat transfer coefficient
            doc.MergeWorksheetCells("F29", "Q29");
            doc.SetCellValue("F29", _overallCalculationViewModel.Overall.k_effective.ToDoubleWithoutDot());

            doc.MergeWorksheetCells("A30", "Q30");
            doc.SetCellStyle("A30", TemaHeaderStyle());
            doc.SetCellValue("A30", "Construction data");

            for (int i = 27; i < 48; i++)
            {
                doc.SetCellValue($"A{4 + i}", $"{i}");
            }

            doc.MergeWorksheetCells("B31", "C31");

            doc.SetCellValue("B32", "Design pressure");
            doc.SetCellValue("C32", "bar-a");

            doc.SetCellValue("B33", "Test pressure");
            doc.SetCellValue("C33", "bar-a");

            doc.SetCellValue("B34", "Design temperature");
            doc.SetCellValue("C34", "°C");

            doc.MergeWorksheetCells("B35", "C35");
            doc.SetCellValue("B35", "Nr. Passes");

            doc.SetCellValue("B36", "Corrosion Allowance");
            doc.SetCellValue("C36", "mm");

            doc.SetCellValue("B37", "OD");
            doc.SetCellValue("C37", "mm");

            doc.SetCellValue("B38", "Thickness");
            doc.SetCellValue("C38", "mm");

            doc.MergeWorksheetCells("B39", "C39");
            doc.SetCellValue("B39", "Nr. Tubes");

            doc.SetCellValue("B40", "Tube length");
            doc.SetCellValue("C40", "mm");

            doc.SetCellValue("B41", "Volume");
            doc.SetCellValue("C41", "l");

            doc.SetCellValue("B42", "Inlet connection (OD)");
            doc.SetCellValue("C42", "mm");

            doc.SetCellValue("B43", "Outlet connection (OD)");
            doc.SetCellValue("C43", "mm");

            doc.MergeWorksheetCells("B44", "C44");
            doc.SetCellValue("B44", "Shell nozzle orientation");

            doc.MergeWorksheetCells("B45", "C45");
            doc.SetCellValue("B45", "Construction material");

            doc.MergeWorksheetCells("B46", "C46");
            doc.SetCellValue("B46", "Gasket material");

            doc.MergeWorksheetCells("B47", "C47");
            doc.SetCellValue("B47", "Nr. Baffles");

            doc.SetCellValue("B48", "Baffle cut (% ID)");
            doc.SetCellValue("C48", "%");

            doc.SetCellValue("B49", "Impingement protection");
            doc.SetCellValue("C49", "Y/N");

            doc.SetCellValue("B50", "Expansion joint fitted");
            doc.SetCellValue("C50", "Y/N");

            doc.MergeWorksheetCells("B51", "C51");
            doc.SetCellValue("B51", "TEMA class");


            doc.SetCellValue("D31", "Shell side");
            doc.SetCellStyle("D31", BoldCenteredBordered());

            doc.SetCellValue("E31", "Tubes side");
            doc.SetCellStyle("E31", BoldCenteredBordered());

            //Design pressure TODO
            doc.SetCellValue("D32", "");
            doc.SetCellValue("E32", "");

            //Test pressure TODO
            doc.SetCellValue("D33", "");
            doc.SetCellValue("E33", "");

            //Design temperature TODO
            doc.SetCellValue("D34", "");
            doc.SetCellValue("E34", "");

            //Nr. Passes
            doc.SetCellValue("D35", _geometryPageViewModel.Geometry.shell_plate_layout_number_of_passes ?? _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);
            doc.SetCellValue("E35", _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);

            //Corrosion Allowance TODO
            doc.SetCellValue("D36", "");
            doc.SetCellValue("E36", "");

            //OD
            doc.SetCellValue("D37", _geometryPageViewModel.Geometry.outer_diameter_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("E37", _geometryPageViewModel.Geometry.outer_diameter_tubes_side.ToDoubleWithoutDot());

            //Thickness
            doc.SetCellValue("D38", _geometryPageViewModel.Geometry.thickness_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("E38", _geometryPageViewModel.Geometry.thickness_tubes_side.ToDoubleWithoutDot());

            //Nr. Tubes
            doc.SetCellValue("E39", _geometryPageViewModel.Geometry.number_of_tubes.ToDoubleWithoutDot());

            //Tube length
            doc.SetCellValue("E40", _geometryPageViewModel.Geometry.tube_inner_length.ToDoubleWithoutDot());

            //Volume
            doc.SetCellValue("D41", _geometryPageViewModel.Geometry.volume_module_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("E41", _geometryPageViewModel.Geometry.volume_module_tubes_side.ToDoubleWithoutDot());

            //Inlet connection (OD)
            doc.SetCellValue("D42", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("E42", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side.ToDoubleWithoutDot());

            //Outlet connection (OD)
            doc.SetCellValue("D43", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("E43", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_tubes_side.ToDoubleWithoutDot());

            //Shell nozzle orientation
            doc.SetCellValue("D44", _geometryPageViewModel.Geometry.shell_nozzle_orientation);

            //Construction material
            doc.SetCellValue("D45", _geometryPageViewModel.Geometry.material_shell_side);
            doc.SetCellValue("E45", _geometryPageViewModel.Geometry.material_tubes_side);

            //Gasket material TODO
            doc.SetCellValue("D46", "");
            doc.SetCellValue("E46", "");

            //Nr. Baffles
            doc.SetCellValue("D47", _bafflesPageViewModel.Baffle.number_of_baffles);

            //Baffle cut (% ID)
            doc.SetCellValue("D48", _bafflesPageViewModel.Baffle.buffle_cut.ToDoubleWithoutDot());

            //Impingement protection TODO
            doc.SetCellValue("D49", "");
            doc.SetCellValue("E49", "");

            //Expansion joint fitted TODO
            doc.SetCellValue("D50", "");
            doc.SetCellValue("E50", "");

            //TEMA class TODO
            doc.SetCellValue("D50", "");
            doc.SetCellValue("E50", "");

            doc.MergeWorksheetCells("F31", "Q51");

            InsertTEMAImage(doc);


            doc.MergeWorksheetCells("A52", "Q52");
            doc.SetCellStyle("A52", TemaHeaderStyle());
            doc.SetCellValue("A52", "Notes");

            doc.MergeWorksheetCells("B53", "Q62");

            for (int i = 48; i <= 57; i++)
            {
                doc.SetCellValue($"A{i + 5}", $"{i}");
            }
        }

        private void InsertTEMAImage(SLDocument doc)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory($"{path}\\Apora");
            DownloadFile(_geometryPageViewModel.Geometry.image_geometry, @$"{path}\Apora\geometry_image.png");
            SLPicture pic = new SLPicture(@$"{path}\\Apora\\geometry_image.png");
            pic.ResizeInPixels(470, 419);
            pic.SetPosition(30, 7);
            doc.InsertPicture(pic);
        }
        #endregion

        #region Graphs
        private static void AddGraphsData(SLDocument doc)
        {
            doc.AddWorksheet("Graphs");
            doc.MergeWorksheetCells("A1", "B1");
            doc.MergeWorksheetCells("C1", "F1");
            doc.MergeWorksheetCells("A2", "B2");
            doc.MergeWorksheetCells("C3", "F3");
            doc.MergeWorksheetCells("A3", "B3");
            doc.MergeWorksheetCells("C4", "F4");
            doc.MergeWorksheetCells("A4", "B4");
            doc.SetCellValue("A1", "Project name");
            doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
            doc.SetCellStyle("A1", "A6", BoldTextStyle());
            doc.SetCellStyle("A1", "F1", BorderCellsStyle());
            doc.SetCellStyle("A2", "C2", BorderCellsStyle());
            doc.SetCellStyle("A3", "F3", BorderCellsStyle());
            doc.SetCellStyle("A4", "F4", BorderCellsStyle());
            doc.SetCellValue("A2", "Revision Nr");
            doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            doc.SetCellValue("A3", "Process");
            doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);
            doc.SetCellValue("A4", "Module name");
            doc.SetCellValue("C4", _overallCalculationViewModel.Name);
            doc.SetCellValue("A6", "Temperatures");
            doc.SetColumnWidth("A", "M", 15);
            TryCreateTempImage(doc);
            doc.SetCellStyle("A28", "J28", WideTextStyle());
            doc.SetCellValue("A28", "Nusselt-Reynolds Tubes Side");
            TryCreateNussretShellSideImage(doc);
            doc.SetCellValue("J28", "Nusselt-Reynolds Shell Side");
            TryCreateNussretTubesSideImage(doc);
        }

        private static void TryCreateNussretTubesSideImage(SLDocument doc)
        {
            var stream = new MemoryStream();
            var pngExporter = new PngExporter { Width = 800, Height = 400 };
            Application.Current.Dispatcher.Invoke(() => pngExporter.Export(_graphsPageViewModel.TubesGraph, stream));
            var pic = new SLPicture(stream.ToArray(), ImagePartType.Png);
            pic.SetPosition(28, 0);
            doc.InsertPicture(pic);
        }

        private static void TryCreateNussretShellSideImage(SLDocument doc)
        {
            var stream = new MemoryStream();
            var pngExporter = new PngExporter { Width = 800, Height = 400 };
            Application.Current.Dispatcher.Invoke(() => pngExporter.Export(_graphsPageViewModel.ShellGraph, stream));
            var pic = new SLPicture(stream.ToArray(), ImagePartType.Png);
            pic.SetPosition(28, 9);
            doc.InsertPicture(pic);
        }

        private static void TryCreateTempImage(SLDocument doc)
        {
            var stream = new MemoryStream();
            var pngExporter = new PngExporter { Width = 800, Height = 400 };
            Application.Current.Dispatcher.Invoke(() => pngExporter.Export(_graphsPageViewModel.Temperatures, stream));
            var pic = new SLPicture(stream.ToArray(), ImagePartType.Png);
            pic.SetPosition(6, 0);
            doc.InsertPicture(pic);
        }
        #endregion

        #region styles

        private static SLStyle WideTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 11;
            style.Font.Bold = true;
            style.SetWrapText(false);
            return style;
        }

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

        private SLStyle BoldCenteredBordered()
        {
            var style = new SLStyle();
            style.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetFontBold(true);
            return style;
        }

        private static SLStyle BorderCellsStyle(bool fromTema = false, bool notCentered = false)
        {
            var style = new SLStyle();
            style.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            if (notCentered)
            {
                style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            }
            else
            {
                style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            }
            if (!fromTema)
            {
                style.SetWrapText(true);
            }
            else
            {
                style.SetWrapText(false);
            }
            return style;
        }

        private SLStyle TemaHeaderStyle()
        {
            var style = new SLStyle();
            style.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.Font.FontSize = 9;
            style.Font.Bold = true;
            style.SetWrapText(false);
            style.Fill.SetGradient(SLGradientShadingStyleValues.FromCenter, SLThemeColorIndexValues.Light2Color, SLThemeColorIndexValues.Light2Color);
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
            doc.SetCellValue("C4", _tubesFluidViewModel.Product?.name);
            doc.MergeWorksheetCells("A5", "B5");
            doc.SetCellStyle("A5", "D5", BorderCellsStyle());
            doc.SetCellValue("A5", "Molar Mass");
            doc.MergeWorksheetCells("A6", "B6");
            doc.SetCellStyle("A6", "C6", BorderCellsStyle());
            doc.SetCellValue("A6", "Pressure");
            doc.SetCellValue("D5", "kg/kmol");
            doc.SetCellValue("C5", _tubesFluidViewModel.Product?.MolarMass);
            doc.SetCellValue("C6", _tubesFluidViewModel.Product?.Pressure);


            CreateHeaders(doc);
            CreateUnits(doc);
            AddData(_tubesFluidViewModel.Product?.product_properties, doc);
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
            doc.SetCellValue("C9", "kJ/kg·°C");
            doc.SetCellValue("D9", "W/m·°C");
            doc.SetCellValue("E9", "cP");
            doc.SetCellValue("F9", "");
            doc.SetCellValue("G9", "J/kg");
            doc.SetCellValue("H9", "kg/m³");
            doc.SetCellValue("I9", "kJ/kg·°C");
            doc.SetCellValue("J9", "W/m·°C");
            doc.SetCellValue("K9", "cP");
            doc.SetCellValue("L9", "bar-a");
            doc.SetCellValue("M9", "%");
        }
        private static void AddData(IEnumerable<ProductProperties> values, SLDocument doc)
        {
            if (values == null)
            {
                return;
            }
            ProductProperties[] properties = values.ToArray();
            for (int i = 0; i < properties.Length; i++)
            {
                doc.SetCellValue($"A{i + 10}", properties[i].liquid_phase_temperature?.ToString("F"));
                doc.SetCellValue($"B{i + 10}", properties[i].liquid_phase_density?.ToString("F"));
                doc.SetCellValue($"C{i + 10}", properties[i].liquid_phase_specific_heat?.ToString("F"));
                doc.SetCellValue($"D{i + 10}", properties[i].liquid_phase_thermal_conductivity?.ToString("F"));
                doc.SetCellValue($"E{i + 10}", properties[i].liquid_phase_consistency_index?.ToString("F"));
                doc.SetCellValue($"F{i + 10}", properties[i].liquid_phase_f_ind?.ToString("F"));
                doc.SetCellValue($"G{i + 10}", properties[i].liquid_phase_dh?.ToString("F"));
                doc.SetCellValue($"H{i + 10}", properties[i].gas_phase_density?.ToString("F"));
                doc.SetCellValue($"I{i + 10}", properties[i].gas_phase_specific_heat?.ToString("F"));
                doc.SetCellValue($"J{i + 10}", properties[i].gas_phase_thermal_conductivity?.ToString("F"));
                doc.SetCellValue($"K{i + 10}", properties[i].gas_phase_dyn_visc_gas?.ToString("F"));
                doc.SetCellValue($"L{i + 10}", properties[i].gas_phase_p_vap?.ToString("F"));
                doc.SetCellValue($"M{i + 10}", properties[i].gas_phase_vapour_frac?.ToString("F"));

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
            doc.SetCellValue("C4", _shellFluidViewModel.Product?.name);
            doc.MergeWorksheetCells("A5", "B5");
            doc.SetCellStyle("A5", "D5", BorderCellsStyle());
            doc.SetCellValue("A5", "Molar Mass");
            doc.MergeWorksheetCells("A6", "B6");
            doc.SetCellStyle("A6", "C6", BorderCellsStyle());
            doc.SetCellValue("A6", "Pressure");
            doc.SetCellValue("D5", "kg/kmol");
            doc.SetCellValue("C5", _shellFluidViewModel.Product?.MolarMass);
            doc.SetCellValue("C6", _shellFluidViewModel.Product?.Pressure);

            CreateHeaders(doc);
            CreateUnits(doc);
            AddData(_shellFluidViewModel.Product?.product_properties, doc);
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
            doc.SetCellValue("C11", _heatBalanceViewModel.Calculation.flow_tube.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("E11", "F11");
            doc.SetCellValue("E11", _heatBalanceViewModel.Calculation.flow_shell.ToDoubleWithoutDot());
            doc.SetCellValue("C12", _heatBalanceViewModel.Calculation.temperature_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D12", _heatBalanceViewModel.Calculation.temperature_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E12", _heatBalanceViewModel.Calculation.temperature_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F12", _heatBalanceViewModel.Calculation.temperature_shell_outlet.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C13", "D13");
            doc.MergeWorksheetCells("E13", "F13");
            doc.SetCellValue("C13", _heatBalanceViewModel.Calculation.duty_tube.ToDoubleWithoutDot());
            doc.SetCellValue("E13", _heatBalanceViewModel.Calculation.duty_shell.ToDoubleWithoutDot());
            doc.SetCellValue("C16", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D16", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E16", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F16", _heatBalanceViewModel.Calculation.liquid_density_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("C21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("C23", _heatBalanceViewModel.Calculation.gas_density_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D23", _heatBalanceViewModel.Calculation.gas_density_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E23", _heatBalanceViewModel.Calculation.gas_density_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F23", _heatBalanceViewModel.Calculation.gas_density_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_outlet.ToDoubleWithoutDot());
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

            AddGeometryTitle("A6", "E6", "Tube & Shell Geometry", doc);

            doc.SetCellValue("C7", "Inner Side");
            doc.SetCellValue("D7", "Tube Side");
            doc.SetCellValue("E7", "Shell Side");

            doc.SetCellStyle("C7", "E7", BorderCellsStyle());
            doc.SetCellStyle("C7", "F7", BoldTextStyle());
            AddGeometryNames(doc);
            AddGeometryTitle("A25", "E25", "Tubeplate Layout", doc);
            AddGeometryTubeplateNames(doc);
            AddGeometryTitle("A37", "E37", "Nozzles", doc);
            AddGeometryNozzlesNames(doc);
            AddGeometryTitle("A50", "E50", "Baffles", doc);
            AddGeometryBafflesNames(doc);
            AddGeometryValues(doc);
            AddGeometryUnits(doc);

            DownloadImage(_geometryPageViewModel.Geometry.image_geometry, doc);

        }

        private static void DownloadImage(string url, SLDocument doc)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory($"{path}\\Apora");
            DownloadFile(_geometryPageViewModel.Geometry.image_geometry, @$"{path}\Apora\geometry_image.png");
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
            doc.SetCellValue("C8", _geometryPageViewModel.Geometry.outer_diameter_inner_side.ToDoubleWithoutDot());
            doc.SetCellValue("D8", _geometryPageViewModel.Geometry.outer_diameter_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E8", _geometryPageViewModel.Geometry.outer_diameter_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("C9", _geometryPageViewModel.Geometry.thickness_inner_side.ToDoubleWithoutDot());
            doc.SetCellValue("D9", _geometryPageViewModel.Geometry.thickness_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E9", _geometryPageViewModel.Geometry.thickness_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("C10", _geometryPageViewModel.Geometry.inner_diameter_inner_side.ToDoubleWithoutDot());
            doc.SetCellValue("D10", _geometryPageViewModel.Geometry.inner_diameter_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E10", _geometryPageViewModel.Geometry.inner_diameter_shell_side.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C11", "D11");
            doc.SetCellValue("C11", _geometryPageViewModel.Geometry.material_tubes_side);
            doc.SetCellValue("E11", _geometryPageViewModel.Geometry.material_shell_side);
            doc.SetCellValue("D12", _geometryPageViewModel.Geometry.number_of_tubes);
            doc.SetCellValue("D13", _geometryPageViewModel.Geometry.tube_inner_length.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C14", "E14");
            doc.SetCellValue("D15", _geometryPageViewModel.Geometry.wetted_perimeter_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E15", _geometryPageViewModel.Geometry.wetted_perimeter_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("D16", _geometryPageViewModel.Geometry.hydraulic_diameter_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E16", _geometryPageViewModel.Geometry.hydraulic_diameter_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("D17", _geometryPageViewModel.Geometry.area_module.ToDoubleWithoutDot());
            doc.SetCellValue("D18", _geometryPageViewModel.Geometry.volume_module_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E18", _geometryPageViewModel.Geometry.volume_module_shell_side.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C19", "D19");
            doc.SetCellValue("C19", _geometryPageViewModel.Geometry.tube_profile_tubes_side.ToNormalCase());
            doc.SetCellValue("D20", _geometryPageViewModel.Geometry.roughness_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E20", _geometryPageViewModel.Geometry.roughness_shell_side.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C21", "E21");
            doc.SetCellValue("C21", _geometryPageViewModel.Geometry.bundle_type.ToNormalCase());
            doc.SetCellValue("C22", _geometryPageViewModel.Geometry.roller_expanded);
            doc.SetCellValue("D26", _geometryPageViewModel.Geometry.tube_plate_layout_tube_pitch.ToDoubleWithoutDot());
            doc.SetCellValue("D27", _geometryPageViewModel.Geometry.tube_plate_layout_tube_layout.ToNormalCase());
            doc.SetCellValue("D28", _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);
            doc.SetCellValue("D29", _geometryPageViewModel.Geometry.tube_plate_layout_div_plate_layout.ToNormalCase());
            doc.SetCellValue("D30", _geometryPageViewModel.Geometry.tube_plate_layout_div_plate_thickness);
            doc.SetCellValue("D31", _geometryPageViewModel.Geometry.tube_plate_layout_tubes_cross_section_pre_pass.ToDoubleWithoutDot());
            doc.SetCellValue("E31", _geometryPageViewModel.Geometry.tube_plate_layout_shell_cross_section.ToDoubleWithoutDot());
            doc.SetCellValue("D32", _geometryPageViewModel.Geometry.tube_plate_layout_perimeter.ToDoubleWithoutDot());
            doc.SetCellValue("D33", _geometryPageViewModel.Geometry.tube_plate_layout_max_nr_tubes);
            doc.SetCellValue("D34", _geometryPageViewModel.Geometry.tube_plate_layout_tube_distribution);
            doc.SetCellValue("D35", _geometryPageViewModel.Geometry.tube_plate_layout_tube_tube_spacing.ToDoubleWithoutDot());
            doc.SetCellValue("C38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_inner_side.ToDoubleWithoutDot());
            doc.SetCellValue("D38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side.ToDoubleWithoutDot());
            //doc.SetCellValue("C39", _geometryPageViewModel.Geometry.nozzles);
            //doc.SetCellValue("D39", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side);
            //doc.SetCellValue("E39", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side);
            doc.SetCellValue("C40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_inner_side.ToDoubleWithoutDot());
            doc.SetCellValue("D40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("D41", _geometryPageViewModel.Geometry.nozzles_in_length_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E41", _geometryPageViewModel.Geometry.nozzles_in_length_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("C42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_inner_side.ToDoubleWithoutDot());
            doc.SetCellValue("D42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("D44", _geometryPageViewModel.Geometry.nozzles_out_inner_diam_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E44", _geometryPageViewModel.Geometry.nozzles_out_inner_diam_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("D45", _geometryPageViewModel.Geometry.nozzles_out_length_tubes_side.ToDoubleWithoutDot());
            doc.SetCellValue("E45", _geometryPageViewModel.Geometry.nozzles_out_length_shell_side.ToDoubleWithoutDot());
            doc.SetCellValue("D46", _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_tubes_side);
            doc.SetCellValue("E46", _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side);
            doc.SetCellValue("D47", _geometryPageViewModel.Geometry.nozzles_number_of_modules_pre_block);
            doc.SetCellValue("D48", _geometryPageViewModel.Geometry.shell_nozzle_orientation.ToNormalCase());
            doc.MergeWorksheetCells("C51", "E51");
            doc.SetCellValue("C51", _bafflesPageViewModel.Baffle.number_of_baffles);
            doc.MergeWorksheetCells("C52", "E52");
            doc.SetCellValue("C52", _bafflesPageViewModel.Baffle.buffle_cut);
            doc.MergeWorksheetCells("C53", "E53");
            doc.SetCellValue("C53", _bafflesPageViewModel.Baffle.inlet_baffle_spacing.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C54", "E54");
            doc.SetCellValue("C54", _bafflesPageViewModel.Baffle.central_baffle_spacing.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C55", "E55");
            doc.SetCellValue("C55", _bafflesPageViewModel.Baffle.outlet_baffle_spacing.ToDoubleWithoutDot());
            doc.MergeWorksheetCells("C56", "E56");
            doc.SetCellValue("C56", _bafflesPageViewModel.Baffle.baffle_thickness);
            doc.MergeWorksheetCells("C57", "E57");
            doc.SetCellValue("C57", _bafflesPageViewModel.Baffle.pairs_of_sealing_strips);
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
            AddHeatTransferUnits(doc);
            AddHeatTransferFlowDataNames(doc);
            AddHeatTransferHeatTransferDataNames(doc);
            AddHeatTransferAreaNames(doc);
            AddHeatTransferTempNames(doc);
            AddHeatTransferPressureDropNames(doc);
            AddHeatTransferVibrationsNames(doc);
            AddHeatTransferData(doc);
            doc.MergeWorksheetCells("C51", "D51");
            doc.SetCellValue("C51", "Use Viscosity Correction");
            doc.SetCellStyle("C51", BoldTextStyle());
            doc.SetCellValue("E51", _overallCalculationViewModel.use_viscosity_correction ? "yes" : "no");
        }

        private static void AddHeatTransferUnits(SLDocument doc)
        {
            doc.SetCellValue("B10", "kg/hr");
            doc.SetCellValue("B11", "°C");
            doc.SetCellValue("B12", "kW");
            doc.SetCellValue("B13", "m/s");
            doc.SetCellValue("B14", "1/s");
            doc.SetCellValue("B17", "cP");
            doc.SetCellValue("B21", "cP");
            doc.SetCellValue("B26", "°C");
            doc.SetCellValue("B27", "°C");
            doc.SetCellValue("B28", "cP");
            doc.SetCellValue("B30", "W/m²·°C");
            doc.SetCellValue("B31", "m²·°C/W");
            doc.SetCellValue("B33", "W/m²·°C");
            doc.SetCellValue("B34", "W/m²·°C");
            doc.SetCellValue("B35", "W/m²·°C");
            doc.SetCellValue("B36", "W/m²·°C");
            doc.SetCellValue("B39", "m²");
            doc.SetCellValue("B40", "m²");
            doc.SetCellValue("B42", "m²");
            doc.SetCellValue("B43", "%");
            doc.SetCellValue("B46", "°C");
            doc.SetCellValue("B48", "°C");
            doc.SetCellValue("B72", "mm");
            doc.SetCellValue("B74", "Hz");
            doc.SetCellValue("B75", "Hz");
            doc.SetCellValue("B77", "m/s");
            doc.SetCellValue("B78", "m/s");
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
            doc.SetCellValue("C10", _overallCalculationViewModel.Overall.flow_tube.ToDoubleWithoutDot());
            doc.SetCellValue("E10", _overallCalculationViewModel.Overall.flow_shell.ToDoubleWithoutDot());
            doc.SetCellValue("C11", _overallCalculationViewModel.Overall.temperature_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D11", _overallCalculationViewModel.Overall.temperature_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E11", _overallCalculationViewModel.Overall.temperature_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F11", _overallCalculationViewModel.Overall.temperature_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C12", _overallCalculationViewModel.Overall.duty_tube.ToDoubleWithoutDot());
            doc.SetCellValue("E12", _overallCalculationViewModel.Overall.duty_shell.ToDoubleWithoutDot());
            doc.SetCellValue("C13", _overallCalculationViewModel.Overall.fluid_velocity_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D13", _overallCalculationViewModel.Overall.fluid_velocity_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E13", _overallCalculationViewModel.Overall.fluid_velocity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F13", _overallCalculationViewModel.Overall.fluid_velocity_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C14", _overallCalculationViewModel.Overall.shear_rate_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D14", _overallCalculationViewModel.Overall.shear_rate_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E14", _overallCalculationViewModel.Overall.shear_rate_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F14", _overallCalculationViewModel.Overall.shear_rate_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C15", _overallCalculationViewModel.Overall.flow_type_tube_inlet);
            doc.SetCellValue("D15", _overallCalculationViewModel.Overall.flow_type_tube_outlet);
            doc.SetCellValue("E15", _overallCalculationViewModel.Overall.flow_type_shell_inlet);
            doc.SetCellValue("F15", _overallCalculationViewModel.Overall.flow_type_shell_outlet);

            doc.SetCellValue("C17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("C18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_shell_outlet.ToDoubleWithoutDot());

            doc.SetCellValue("C21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C22", _overallCalculationViewModel.Overall.gas_phase_reynolds_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D22", _overallCalculationViewModel.Overall.gas_phase_reynolds_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E22", _overallCalculationViewModel.Overall.gas_phase_reynolds_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F22", _overallCalculationViewModel.Overall.gas_phase_reynolds_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C23", _overallCalculationViewModel.Overall.gas_phase_prandtl_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D23", _overallCalculationViewModel.Overall.gas_phase_prandtl_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E23", _overallCalculationViewModel.Overall.gas_phase_prandtl_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F23", _overallCalculationViewModel.Overall.gas_phase_prandtl_shell_outlet.ToDoubleWithoutDot());

            doc.SetCellValue("C26", _overallCalculationViewModel.Overall.wall_temperature_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D26", _overallCalculationViewModel.Overall.wall_temperature_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E26", _overallCalculationViewModel.Overall.wall_temperature_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F26", _overallCalculationViewModel.Overall.wall_temperature_shell_outlet.ToDoubleWithoutDot());

            //TODO: Добавить свойство average metall temp

            doc.SetCellValue("C28", _overallCalculationViewModel.Overall.wall_consistency_index_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D28", _overallCalculationViewModel.Overall.wall_consistency_index_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E28", _overallCalculationViewModel.Overall.wall_consistency_index_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F28", _overallCalculationViewModel.Overall.wall_consistency_index_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("C29", _overallCalculationViewModel.Overall.nusselt_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D29", _overallCalculationViewModel.Overall.nusselt_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E29", _overallCalculationViewModel.Overall.nusselt_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F29", _overallCalculationViewModel.Overall.nusselt_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C30", _overallCalculationViewModel.Overall.k_side_tube_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("D30", _overallCalculationViewModel.Overall.k_side_tube_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("E30", _overallCalculationViewModel.Overall.k_side_shell_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("F30", _overallCalculationViewModel.Overall.k_side_shell_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C31", _overallCalculationViewModel.Overall.fouling_factor_tube.ToDoubleWithoutDot());
            doc.SetCellValue("E31", _overallCalculationViewModel.Overall.fouling_factor_shell.ToDoubleWithoutDot());
            doc.SetCellValue("C32", "Inlet");
            doc.SetCellValue("E32", "Outlet");
            doc.SetCellValue("C33", _overallCalculationViewModel.Overall.k_unfouled_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("E33", _overallCalculationViewModel.Overall.k_unfouled_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C34", _overallCalculationViewModel.Overall.k_fouled_inlet.ToDoubleWithoutDot());
            doc.SetCellValue("E34", _overallCalculationViewModel.Overall.k_fouled_outlet.ToDoubleWithoutDot());
            doc.SetCellValue("C35", _overallCalculationViewModel.Overall.k_global_fouled.ToDoubleWithoutDot());
            doc.SetCellValue("C36", _overallCalculationViewModel.Overall.k_effective.ToDoubleWithoutDot());
            doc.SetCellValue("C39", _overallCalculationViewModel.Overall.surface_area_required.ToDoubleWithoutDot());
            doc.SetCellValue("C40", _overallCalculationViewModel.Overall.area_module.ToDoubleWithoutDot());
            doc.SetCellValue("C41", _overallCalculationViewModel.Overall.nr_modules.ToDoubleWithoutDot());
            doc.SetCellValue("C42", _overallCalculationViewModel.Overall.area_fitted.ToDoubleWithoutDot());
            doc.SetCellValue("C43", _overallCalculationViewModel.Overall.excess_area.ToDoubleWithoutDot());
            doc.SetCellValue("C46", _overallCalculationViewModel.Overall.LMTD.ToDoubleWithoutDot());
            doc.SetCellValue("C47", _overallCalculationViewModel.Overall.LMTD_correction_factor);
            doc.SetCellValue("C48", _overallCalculationViewModel.Overall.adjusted_LMTD.ToDoubleWithoutDot());
            doc.SetCellValue("C54", _overallCalculationViewModel.Overall.pressure_drop_tube_side_modules_V.ToDoubleWithoutDot());
            doc.SetCellValue("E54", _overallCalculationViewModel.Overall.pressure_drop_tube_side_modules_P.ToDoubleWithoutDot());
            doc.SetCellValue("C55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_V.ToDoubleWithoutDot());
            doc.SetCellValue("D55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_pV.ToDoubleWithoutDot());
            doc.SetCellValue("E55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_P.ToDoubleWithoutDot());
            doc.SetCellValue("C56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_V.ToDoubleWithoutDot());
            doc.SetCellValue("D56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_pV.ToDoubleWithoutDot());
            doc.SetCellValue("E56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_P.ToDoubleWithoutDot());
            doc.SetCellValue("C57", _overallCalculationViewModel.Overall.pressure_drop_tube_side_bends_V.ToDoubleWithoutDot());
            doc.SetCellValue("E57", _overallCalculationViewModel.Overall.pressure_drop_tube_side_bends_P.ToDoubleWithoutDot());
            doc.SetCellValue("E58", _overallCalculationViewModel.Overall.pressure_drop_tube_side_total_P.ToDoubleWithoutDot());
            doc.SetCellValue("C62", _overallCalculationViewModel.Overall.pressure_drop_shell_side_modules_V.ToDoubleWithoutDot());
            doc.SetCellValue("E62", _overallCalculationViewModel.Overall.pressure_drop_shell_side_modules_P.ToDoubleWithoutDot());
            doc.SetCellValue("C63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_V.ToDoubleWithoutDot());
            doc.SetCellValue("D63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_pV.ToDoubleWithoutDot());
            doc.SetCellValue("E63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_P.ToDoubleWithoutDot());

            doc.SetCellValue("C64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_V.ToDoubleWithoutDot());
            doc.SetCellValue("D64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_pV.ToDoubleWithoutDot());
            doc.SetCellValue("E64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_P.ToDoubleWithoutDot());
            //doc.SetCellValue("E65", _overallCalculationViewModel.Overall.win);
            doc.SetCellValue("E68", _overallCalculationViewModel.Overall.pressure_drop_shell_side_total_p.ToDoubleWithoutDot());
            doc.SetCellValue("C72", _overallCalculationViewModel.Overall.vibrations_inlet_span_length.ToDoubleWithoutDot());
            doc.SetCellValue("D72", _overallCalculationViewModel.Overall.vibrations_central_span_length.ToDoubleWithoutDot());
            doc.SetCellValue("E72", _overallCalculationViewModel.Overall.vibrations_outlet_span_length.ToDoubleWithoutDot());
            doc.SetCellValue("C73", _overallCalculationViewModel.Overall.vibrations_inlet_span_length_tema_lb.ToDoubleWithoutDot());
            doc.SetCellValue("D73", _overallCalculationViewModel.Overall.vibrations_central_span_length_tema_lb.ToDoubleWithoutDot());
            doc.SetCellValue("E73", _overallCalculationViewModel.Overall.vibrations_outlet_span_length_tema_lb.ToDoubleWithoutDot());
            doc.SetCellValue("C74", _overallCalculationViewModel.Overall.vibrations_inlet_tubes_natural_frequency.ToDoubleWithoutDot());
            doc.SetCellValue("D74", _overallCalculationViewModel.Overall.vibrations_central_tubes_natural_frequency.ToDoubleWithoutDot());
            doc.SetCellValue("E74", _overallCalculationViewModel.Overall.vibrations_outlet_tubes_natural_frequency.ToDoubleWithoutDot());
            doc.SetCellValue("C75", _overallCalculationViewModel.Overall.vibrations_inlet_shell_acoustic_frequency_gases.ToDoubleWithoutDot());
            doc.SetCellValue("D75", _overallCalculationViewModel.Overall.vibrations_central_shell_acoustic_frequency_gases.ToDoubleWithoutDot());
            doc.SetCellValue("E75", _overallCalculationViewModel.Overall.vibrations_outlet_shell_acoustic_frequency_gases.ToDoubleWithoutDot());

            doc.SetCellValue("C77", _overallCalculationViewModel.Overall.vibrations_inlet_cross_flow_velocity.ToDoubleWithoutDot());
            doc.SetCellValue("D77", _overallCalculationViewModel.Overall.vibrations_central_cross_flow_velocity.ToDoubleWithoutDot());
            doc.SetCellValue("E77", _overallCalculationViewModel.Overall.vibrations_outlet_cross_flow_velocity.ToDoubleWithoutDot());
            doc.SetCellValue("C78", _overallCalculationViewModel.Overall.vibrations_inlet_cricical_velocity.ToDoubleWithoutDot());
            doc.SetCellValue("D78", _overallCalculationViewModel.Overall.vibrations_central_cricical_velocity.ToDoubleWithoutDot());
            doc.SetCellValue("E78", _overallCalculationViewModel.Overall.vibrations_outlet_cricical_velocity.ToDoubleWithoutDot());
            doc.SetCellValue("C79", _overallCalculationViewModel.Overall.vibrations_inlet_average_cross_flow_velocity_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("D79", _overallCalculationViewModel.Overall.vibrations_central_average_cross_flow_velocity_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("E79", _overallCalculationViewModel.Overall.vibrations_outlet_average_cross_flow_velocity_ratio.ToDoubleWithoutDot());

            doc.SetCellValue("C81", _overallCalculationViewModel.Overall.vibrations_inlet_vortex_shedding_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("D81", _overallCalculationViewModel.Overall.vibrations_central_vortex_shedding_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("E81", _overallCalculationViewModel.Overall.vibrations_outlet_vortex_shedding_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("C82", _overallCalculationViewModel.Overall.vibrations_inlet_turbulent_buffeting_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("D82", _overallCalculationViewModel.Overall.vibrations_central_turbulent_buffeting_ratio.ToDoubleWithoutDot());
            doc.SetCellValue("E82", _overallCalculationViewModel.Overall.vibrations_outlet_turbulent_buffeting_ratio.ToDoubleWithoutDot());

            doc.SetCellValue("C84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_inlet == 1 ? "Yes" : "No");
            doc.SetCellValue("D84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_central == 1 ? "Yes" : "No");
            doc.SetCellValue("E84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_outlet == 1 ? "Yes" : "No");


            doc.SetCellValue("F73", "<1,0");
            doc.SetCellValue("F79", "<0,8");
            doc.SetCellValue("F81", "<1,0");
            doc.SetCellValue("F82", "<1,0");

            doc.SetCellStyle("C71", "E71", BoldTextStyle());
            doc.SetCellStyle("C71", "E71", BorderCellsStyle());

            doc.SetCellValue("C71", "Inlet");
            doc.SetCellValue("D71", "Central");
            doc.SetCellValue("E71", "Outlet");
        }

        #endregion

        private static void DownloadFile(string reference, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var s = client.GetStreamAsync(reference))
                {
                    using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        s.Result.CopyTo(fs);
                    }
                }
            }
        }
    }
}
