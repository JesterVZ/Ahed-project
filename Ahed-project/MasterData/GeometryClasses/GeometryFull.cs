using Ahed_project.Pages;
using Ahed_project.Services.Global;
using Ahed_project.Settings;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.GeometryClasses
{
    public class GeometryFull : INotifyPropertyChanged
    {
        private int _geometry_catalog_id;
        private int _geometry_id;
        private int _calculation_id;
        private int _project_id;
        private string _name;
        private string _head_exchange_type;
        private string _owner;
        private string _comment;
        private DateTime? _date_created;
        private string _outer_diameter_inner_side;
        private string _outer_diameter_tubes_side;
        private string _outer_diameter_shell_side;
        private string _thickness_inner_side;
        private string _thickness_tubes_side;
        private string _thickness_shell_side;
        private string _inner_diameter_inner_side;
        private string _inner_diameter_tubes_side;
        private string _inner_diameter_shell_side;
        private string _material_tubes_side;
        private string _material_shell_side;
        private string _number_of_tubes;
        private string _tube_inner_length;
        private string _orientation;
        private string _wetted_perimeter_tubes_side;
        private string _wetted_perimeter_shell_side;
        private string _hydraulic_diameter_tubes_side;
        private string _hydraulic_diameter_shell_side;
        private string _area_module;
        private string _volume_module_tubes_side;
        private string _volume_module_shell_side;
        private string _tube_profile_tubes_side;
        private string _roughness_tubes_side;
        private string _roughness_shell_side;
        private string _scraping_frequency_tubes_side;
        private string _motor_power_tubes_side;
        private string _bundle_type;
        private string _roller_expanded;
        private string _tube_plate_layout_tube_pitch;
        private string _tube_plate_layout_tube_layout;
        private string _tube_plate_layout_number_of_passes;
        private string _tube_plate_layout_div_plate_layout;
        private string _tube_plate_layout_sealing_type;
        private string _tube_plate_layout_housings_space;
        private string _tube_plate_layout_div_plate_thickness;
        private string _tube_plate_layout_tubes_cross_section_pre_pass;
        private string _tube_plate_layout_shell_cross_section;
        private string _tube_plate_layout_tubeplate_thickness;
        private string _tube_plate_layout_perimeter;
        private string _tube_plate_layout_max_nr_tubes;
        private string _tube_plate_layout_tube_distribution;
        private string _tube_plate_layout_tube_tube_spacing;
        private string _nozzles_in_outer_diam_inner_side;
        private string _nozzles_in_outer_diam_tubes_side;
        private string _nozzles_in_outer_diam_shell_side;
        private string _nozzles_in_length_tubes_side;
        private string _nozzles_in_length_shell_side;
        private string _nozzles_in_thickness_inner_side;
        private string _nozzles_in_thickness_tubes_side;
        private string _nozzles_out_length_tubes_side;
        private string _nozzles_out_length_shell_side;
        private string _nozzles_in_thickness_shell_side;
        private string _nozzles_in_inner_diam_inner_side;
        private string _nozzles_in_inner_diam_tubes_side;
        private string _nozzles_in_inner_diam_shell_side;
        private string _nozzles_out_outer_diam_inner_side;
        private string _nozzles_out_outer_diam_tubes_side;
        private string _nozzles_out_outer_diam_shell_side;
        private string _nozzles_out_thickness_inner_side;
        private string _nozzles_out_thickness_tubes_side;
        private string _nozzles_out_thickness_shell_side;
        private string _nozzles_out_inner_diam_inner_side;
        private string _nozzles_out_inner_diam_tubes_side;
        private string _nozzles_out_inner_diam_shell_side;
        private string _nozzles_number_of_parallel_lines_tubes_side;
        private string _nozzles_number_of_parallel_lines_shell_side;
        private string _nozzles_number_of_modules_pre_block;
        private string _shell_nozzle_orientation;
        private string _nr_baffles;
        private string _baffle_cut;
        private string _inlet_baffle_spacing;
        private string _central_baffle_spacing;
        private string _outlet_baffle_spacing;
        private string _baffle_thickness;
        private string _pairs_of_sealing_strips;
        private string _clearances_spacing_tube_to_tubeplate;
        private string _clearances_spacing_tubeplate_to_shell;
        private string _clearances_spacing_division_plate_to_shell;
        private string _clearances_spacing_minimum_tube_hole_to_tubeplate_edge;
        private string _clearances_spacing_min_tube_hole_to_division_plate_groove;
        private string _clearances_spacing_division_plate_to_tubeplate;
        private string _clearances_spacing_minimum_tube_in_tube_spacing;
        private string _clearances_spacing_actual_tube_hole_to_tubeplate_edge;
        private string _clearances_spacing_actual_tube_hole_to_tube_hole;
        private string _diametral_clearance_shell_baffle;
        private string _diametral_clearance_tube_baffle;
        private string _image_geometry;
        private DateTime? _createdAt;
        private DateTime? _updatedAt;
        public int geometry_catalog_id { get => _geometry_catalog_id; set { _geometry_catalog_id = value; OnPropertyChanged(nameof(geometry_catalog_id)); } }
        public int geometry_id { get => _geometry_id; set { _geometry_id = value; OnPropertyChanged(nameof(geometry_id)); } }
        public int calculation_id { get => _calculation_id; set { _calculation_id = value; OnPropertyChanged(nameof(calculation_id)); } }
        public int project_id { get => _project_id; set { _project_id = value; OnPropertyChanged(nameof(project_id)); } }
        public string name 
        {
            get => _name; set
            { 
                _name = value;
                OnPropertyChanged(nameof(name));
                GlobalFunctionsAndCallersService.UpdateNameInOverall(value);
            } 
        }
        public string head_exchange_type { get => _head_exchange_type; set { _head_exchange_type = value; OnPropertyChanged(nameof(head_exchange_type)); } }
        public string owner { get => _owner; set { _owner = value; OnPropertyChanged(nameof(owner)); } }
        public string comment { get => _comment; set { _comment = value; OnPropertyChanged(nameof(comment)); } }
        public DateTime? date_created { get => _date_created; set { _date_created = value; OnPropertyChanged(nameof(date_created)); } }
        public string outer_diameter_inner_side { get => _outer_diameter_inner_side; set { _outer_diameter_inner_side = value; OnPropertyChanged(nameof(outer_diameter_inner_side)); } }
        public string outer_diameter_tubes_side { get => _outer_diameter_tubes_side; set { _outer_diameter_tubes_side = value; OnPropertyChanged(nameof(outer_diameter_tubes_side)); } }
        public string outer_diameter_shell_side { get => _outer_diameter_shell_side; set { _outer_diameter_shell_side = value; OnPropertyChanged(nameof(outer_diameter_shell_side)); } }
        public string thickness_inner_side { get => _thickness_inner_side; set { _thickness_inner_side = value; OnPropertyChanged(nameof(thickness_inner_side)); } }
        public string thickness_tubes_side { get => _thickness_tubes_side; set { _thickness_tubes_side = value; OnPropertyChanged(nameof(thickness_tubes_side)); } }
        public string thickness_shell_side { get => _thickness_shell_side; set { _thickness_shell_side = value; OnPropertyChanged(nameof(thickness_shell_side)); } }
        public string inner_diameter_inner_side { get => _inner_diameter_inner_side; set { _inner_diameter_inner_side = value; OnPropertyChanged(nameof(inner_diameter_inner_side)); } }
        public string inner_diameter_tubes_side { get => _inner_diameter_tubes_side; set { _inner_diameter_tubes_side = value; OnPropertyChanged(nameof(inner_diameter_tubes_side)); } }
        public string inner_diameter_shell_side { get => _inner_diameter_shell_side; set { _inner_diameter_shell_side = value; OnPropertyChanged(nameof(inner_diameter_shell_side)); } }
        public string material_tubes_side { get => _material_tubes_side; set { _material_tubes_side = value; OnPropertyChanged(nameof(material_tubes_side)); } }
        public string material_shell_side { get => _material_shell_side; set { _material_shell_side = value; OnPropertyChanged(nameof(material_shell_side)); } }
        public string number_of_tubes { get => _number_of_tubes; set { _number_of_tubes = value; OnPropertyChanged(nameof(number_of_tubes)); } }
        public string tube_inner_length { get => _tube_inner_length; set { _tube_inner_length = value; OnPropertyChanged(nameof(tube_inner_length)); } }
        public string orientation { get => _orientation; set { _orientation = value; OnPropertyChanged(nameof(orientation)); } }
        public string wetted_perimeter_tubes_side { get => _wetted_perimeter_tubes_side; set { _wetted_perimeter_tubes_side = value; OnPropertyChanged(nameof(wetted_perimeter_tubes_side)); } }
        public string wetted_perimeter_shell_side { get => _wetted_perimeter_shell_side; set { _wetted_perimeter_shell_side = value; OnPropertyChanged(nameof(wetted_perimeter_shell_side)); } }
        public string hydraulic_diameter_tubes_side { get => _hydraulic_diameter_tubes_side; set { _hydraulic_diameter_tubes_side = value; OnPropertyChanged(nameof(hydraulic_diameter_tubes_side)); } }
        public string hydraulic_diameter_shell_side { get => _hydraulic_diameter_shell_side; set { _hydraulic_diameter_shell_side = value; OnPropertyChanged(nameof(hydraulic_diameter_shell_side)); } }
        public string area_module { get => _area_module; set { _area_module = value; OnPropertyChanged(nameof(area_module)); } }
        public string volume_module_tubes_side { get => _volume_module_tubes_side; set { _volume_module_tubes_side = value; OnPropertyChanged(nameof(volume_module_tubes_side)); } }
        public string volume_module_shell_side { get => _volume_module_shell_side; set { _volume_module_shell_side = value; OnPropertyChanged(nameof(volume_module_shell_side)); } }
        public string tube_profile_tubes_side { get => _tube_profile_tubes_side; set { _tube_profile_tubes_side = value; OnPropertyChanged(nameof(tube_profile_tubes_side)); } }
        public string roughness_tubes_side { get => _roughness_tubes_side; set { _roughness_tubes_side = value; OnPropertyChanged(nameof(roughness_tubes_side)); } }
        public string roughness_shell_side { get => _roughness_shell_side; set { _roughness_shell_side = value; OnPropertyChanged(nameof(roughness_shell_side)); } }
        public string scraping_frequency_tubes_side { get => _scraping_frequency_tubes_side; set { _scraping_frequency_tubes_side = value; OnPropertyChanged(nameof(scraping_frequency_tubes_side)); } }
        public string motor_power_tubes_side { get => _motor_power_tubes_side; set { _motor_power_tubes_side = value; OnPropertyChanged(nameof(motor_power_tubes_side)); } }
        public string bundle_type { get => _bundle_type; set { _bundle_type = value; OnPropertyChanged(nameof(bundle_type)); } }
        public string roller_expanded { get => _roller_expanded; set { _roller_expanded = value; OnPropertyChanged(nameof(roller_expanded)); } }
        public string tube_plate_layout_tube_pitch { get => _tube_plate_layout_tube_pitch; set { _tube_plate_layout_tube_pitch = value; OnPropertyChanged(nameof(tube_plate_layout_tube_pitch)); } }
        public string tube_plate_layout_tube_layout { get => _tube_plate_layout_tube_layout; set { _tube_plate_layout_tube_layout = value; OnPropertyChanged(nameof(tube_plate_layout_tube_layout)); } }
        public string tube_plate_layout_number_of_passes { get => _tube_plate_layout_number_of_passes; set { _tube_plate_layout_number_of_passes = value; OnPropertyChanged(nameof(tube_plate_layout_number_of_passes)); } }
        public string tube_plate_layout_div_plate_layout { get => _tube_plate_layout_div_plate_layout; set { _tube_plate_layout_div_plate_layout = value; OnPropertyChanged(nameof(tube_plate_layout_div_plate_layout)); } }
        public string tube_plate_layout_sealing_type { get => _tube_plate_layout_sealing_type; set { _tube_plate_layout_sealing_type = value; OnPropertyChanged(nameof(tube_plate_layout_sealing_type)); } }
        public string tube_plate_layout_housings_space { get => _tube_plate_layout_housings_space; set { _tube_plate_layout_housings_space = value; OnPropertyChanged(nameof(tube_plate_layout_housings_space)); } }
        public string tube_plate_layout_div_plate_thickness { get => _tube_plate_layout_div_plate_thickness; set { _tube_plate_layout_div_plate_thickness = value; OnPropertyChanged(nameof(tube_plate_layout_div_plate_thickness)); } }
        public string tube_plate_layout_tubes_cross_section_pre_pass { get => _tube_plate_layout_tubes_cross_section_pre_pass; set { _tube_plate_layout_tubes_cross_section_pre_pass = value; OnPropertyChanged(nameof(tube_plate_layout_tubes_cross_section_pre_pass)); } }
        public string tube_plate_layout_shell_cross_section { get => _tube_plate_layout_shell_cross_section; set { _tube_plate_layout_shell_cross_section = value; OnPropertyChanged(nameof(tube_plate_layout_shell_cross_section)); } }
        public string tube_plate_layout_tubeplate_thickness { get => _tube_plate_layout_tubeplate_thickness; set { _tube_plate_layout_tubeplate_thickness = value; OnPropertyChanged(nameof(tube_plate_layout_tubeplate_thickness)); } }
        public string tube_plate_layout_perimeter { get => _tube_plate_layout_perimeter; set { _tube_plate_layout_perimeter = value; OnPropertyChanged(nameof(tube_plate_layout_perimeter)); } }
        public string tube_plate_layout_max_nr_tubes { get => _tube_plate_layout_max_nr_tubes; set { _tube_plate_layout_max_nr_tubes = value; OnPropertyChanged(nameof(tube_plate_layout_max_nr_tubes)); } }
        public string tube_plate_layout_tube_distribution { get => _tube_plate_layout_tube_distribution; set { _tube_plate_layout_tube_distribution = value; OnPropertyChanged(nameof(tube_plate_layout_tube_distribution)); } }
        public string tube_plate_layout_tube_tube_spacing { get => _tube_plate_layout_tube_tube_spacing; set { _tube_plate_layout_tube_tube_spacing = value; OnPropertyChanged(nameof(tube_plate_layout_tube_tube_spacing)); } }
        public string nozzles_in_outer_diam_inner_side { get => _nozzles_in_outer_diam_inner_side; set { _nozzles_in_outer_diam_inner_side = value; OnPropertyChanged(nameof(nozzles_in_outer_diam_inner_side)); } }
        public string nozzles_in_outer_diam_tubes_side { get => _nozzles_in_outer_diam_tubes_side; set { _nozzles_in_outer_diam_tubes_side = value; OnPropertyChanged(nameof(nozzles_in_outer_diam_tubes_side)); } }
        public string nozzles_in_outer_diam_shell_side { get => _nozzles_in_outer_diam_shell_side; set { _nozzles_in_outer_diam_shell_side = value; OnPropertyChanged(nameof(nozzles_in_outer_diam_shell_side)); } }
        public string nozzles_in_length_tubes_side { get => _nozzles_in_length_tubes_side; set { _nozzles_in_length_tubes_side = value; OnPropertyChanged(nameof(nozzles_in_length_tubes_side)); } }
        public string nozzles_in_length_shell_side { get => _nozzles_in_length_shell_side; set { _nozzles_in_length_shell_side = value; OnPropertyChanged(nameof(nozzles_in_length_shell_side)); } }
        public string nozzles_in_thickness_inner_side { get => _nozzles_in_thickness_inner_side; set { _nozzles_in_thickness_inner_side = value; OnPropertyChanged(nameof(nozzles_in_thickness_inner_side)); } }
        public string nozzles_in_thickness_tubes_side { get => _nozzles_in_thickness_tubes_side; set { _nozzles_in_thickness_tubes_side = value; OnPropertyChanged(nameof(nozzles_in_thickness_tubes_side)); } }
        public string nozzles_out_length_tubes_side { get => _nozzles_out_length_tubes_side; set { _nozzles_out_length_tubes_side = value; OnPropertyChanged(nameof(nozzles_out_length_tubes_side)); } }
        public string nozzles_out_length_shell_side { get => _nozzles_out_length_shell_side; set { _nozzles_out_length_shell_side = value; OnPropertyChanged(nameof(nozzles_out_length_shell_side)); } }
        public string nozzles_in_thickness_shell_side { get => _nozzles_in_thickness_shell_side; set { _nozzles_in_thickness_shell_side = value; OnPropertyChanged(nameof(nozzles_in_thickness_shell_side)); } }
        public string nozzles_in_inner_diam_inner_side { get => _nozzles_in_inner_diam_inner_side; set { _nozzles_in_inner_diam_inner_side = value; OnPropertyChanged(nameof(nozzles_in_inner_diam_inner_side)); } }
        public string nozzles_in_inner_diam_tubes_side { get => _nozzles_in_inner_diam_tubes_side; set { _nozzles_in_inner_diam_tubes_side = value; OnPropertyChanged(nameof(nozzles_in_inner_diam_tubes_side)); } }
        public string nozzles_in_inner_diam_shell_side { get => _nozzles_in_inner_diam_shell_side; set { _nozzles_in_inner_diam_shell_side = value; OnPropertyChanged(nameof(nozzles_in_inner_diam_shell_side)); } }
        public string nozzles_out_outer_diam_inner_side { get => _nozzles_out_outer_diam_inner_side; set { _nozzles_out_outer_diam_inner_side = value; OnPropertyChanged(nameof(nozzles_out_outer_diam_inner_side)); } }
        public string nozzles_out_outer_diam_tubes_side { get => _nozzles_out_outer_diam_tubes_side; set { _nozzles_out_outer_diam_tubes_side = value; OnPropertyChanged(nameof(nozzles_out_outer_diam_tubes_side)); } }
        public string nozzles_out_outer_diam_shell_side { get => _nozzles_out_outer_diam_shell_side; set { _nozzles_out_outer_diam_shell_side = value; OnPropertyChanged(nameof(nozzles_out_outer_diam_shell_side)); } }
        public string nozzles_out_thickness_inner_side { get => _nozzles_out_thickness_inner_side; set { _nozzles_out_thickness_inner_side = value; OnPropertyChanged(nameof(nozzles_out_thickness_inner_side)); } }
        public string nozzles_out_thickness_tubes_side { get => _nozzles_out_thickness_tubes_side; set { _nozzles_out_thickness_tubes_side = value; OnPropertyChanged(nameof(nozzles_out_thickness_tubes_side)); } }
        public string nozzles_out_thickness_shell_side { get => _nozzles_out_thickness_shell_side; set { _nozzles_out_thickness_shell_side = value; OnPropertyChanged(nameof(nozzles_out_thickness_shell_side)); } }
        public string nozzles_out_inner_diam_inner_side { get => _nozzles_out_inner_diam_inner_side; set { _nozzles_out_inner_diam_inner_side = value; OnPropertyChanged(nameof(nozzles_out_inner_diam_inner_side)); } }
        public string nozzles_out_inner_diam_tubes_side { get => _nozzles_out_inner_diam_tubes_side; set { _nozzles_out_inner_diam_tubes_side = value; OnPropertyChanged(nameof(nozzles_out_inner_diam_tubes_side)); } }
        public string nozzles_out_inner_diam_shell_side { get => _nozzles_out_inner_diam_shell_side; set { _nozzles_out_inner_diam_shell_side = value; OnPropertyChanged(nameof(nozzles_out_inner_diam_shell_side)); } }
        public string nozzles_number_of_parallel_lines_tubes_side { get => _nozzles_number_of_parallel_lines_tubes_side; set { _nozzles_number_of_parallel_lines_tubes_side = value; OnPropertyChanged(nameof(nozzles_number_of_parallel_lines_tubes_side)); } }
        public string nozzles_number_of_parallel_lines_shell_side { get => _nozzles_number_of_parallel_lines_shell_side; set { _nozzles_number_of_parallel_lines_shell_side = value; OnPropertyChanged(nameof(nozzles_number_of_parallel_lines_shell_side)); } }
        public string nozzles_number_of_modules_pre_block { get => _nozzles_number_of_modules_pre_block; set { _nozzles_number_of_modules_pre_block = value; OnPropertyChanged(nameof(nozzles_number_of_modules_pre_block)); } }
        public string shell_nozzle_orientation { get => _shell_nozzle_orientation; set { _shell_nozzle_orientation = value; OnPropertyChanged(nameof(shell_nozzle_orientation)); } }
        public string nr_baffles { get => _nr_baffles; set { _nr_baffles = value; OnPropertyChanged(nameof(nr_baffles)); } }
        public string baffle_cut { get => _baffle_cut; set { _baffle_cut = value; OnPropertyChanged(nameof(baffle_cut)); } }
        public string inlet_baffle_spacing { get => _inlet_baffle_spacing; set { _inlet_baffle_spacing = value; OnPropertyChanged(nameof(inlet_baffle_spacing)); } }
        public string central_baffle_spacing { get => _central_baffle_spacing; set { _central_baffle_spacing = value; OnPropertyChanged(nameof(central_baffle_spacing)); } }
        public string outlet_baffle_spacing { get => _outlet_baffle_spacing; set { _outlet_baffle_spacing = value; OnPropertyChanged(nameof(outlet_baffle_spacing)); } }
        public string baffle_thickness { get => _baffle_thickness; set { _baffle_thickness = value; OnPropertyChanged(nameof(baffle_thickness)); } }
        public string pairs_of_sealing_strips { get => _pairs_of_sealing_strips; set { _pairs_of_sealing_strips = value; OnPropertyChanged(nameof(pairs_of_sealing_strips)); } }
        public string clearances_spacing_tube_to_tubeplate { get => _clearances_spacing_tube_to_tubeplate; set { _clearances_spacing_tube_to_tubeplate = value; OnPropertyChanged(nameof(clearances_spacing_tube_to_tubeplate)); } }
        public string clearances_spacing_tubeplate_to_shell { get => _clearances_spacing_tubeplate_to_shell; set { _clearances_spacing_tubeplate_to_shell = value; OnPropertyChanged(nameof(clearances_spacing_tubeplate_to_shell)); } }
        public string clearances_spacing_division_plate_to_shell { get => _clearances_spacing_division_plate_to_shell; set { _clearances_spacing_division_plate_to_shell = value; OnPropertyChanged(nameof(clearances_spacing_division_plate_to_shell)); } }
        public string clearances_spacing_minimum_tube_hole_to_tubeplate_edge { get => _clearances_spacing_minimum_tube_hole_to_tubeplate_edge; set { _clearances_spacing_minimum_tube_hole_to_tubeplate_edge = value; OnPropertyChanged(nameof(clearances_spacing_minimum_tube_hole_to_tubeplate_edge)); } }
        public string clearances_spacing_min_tube_hole_to_division_plate_groove { get => _clearances_spacing_min_tube_hole_to_division_plate_groove; set { _clearances_spacing_min_tube_hole_to_division_plate_groove = value; OnPropertyChanged(nameof(clearances_spacing_min_tube_hole_to_division_plate_groove)); } }
        public string clearances_spacing_division_plate_to_tubeplate { get => _clearances_spacing_division_plate_to_tubeplate; set { _clearances_spacing_division_plate_to_tubeplate = value; OnPropertyChanged(nameof(clearances_spacing_division_plate_to_tubeplate)); } }
        public string clearances_spacing_minimum_tube_in_tube_spacing { get => _clearances_spacing_minimum_tube_in_tube_spacing; set { _clearances_spacing_minimum_tube_in_tube_spacing = value; OnPropertyChanged(nameof(clearances_spacing_minimum_tube_in_tube_spacing)); } }
        public string clearances_spacing_actual_tube_hole_to_tubeplate_edge { get => _clearances_spacing_actual_tube_hole_to_tubeplate_edge; set { _clearances_spacing_actual_tube_hole_to_tubeplate_edge = value; OnPropertyChanged(nameof(clearances_spacing_actual_tube_hole_to_tubeplate_edge)); } }
        public string clearances_spacing_actual_tube_hole_to_tube_hole { get => _clearances_spacing_actual_tube_hole_to_tube_hole; set { _clearances_spacing_actual_tube_hole_to_tube_hole = value; OnPropertyChanged(nameof(clearances_spacing_actual_tube_hole_to_tube_hole)); } }
        public string image_geometry { get => _image_geometry; set { _image_geometry = value; OnPropertyChanged(nameof(image_geometry)); } }
        public string diametral_clearance_shell_baffle { get => _diametral_clearance_shell_baffle; set { _diametral_clearance_shell_baffle = value; OnPropertyChanged(nameof(diametral_clearance_shell_baffle)); } }
        public string diametral_clearance_tube_baffle { get => _diametral_clearance_tube_baffle; set { _diametral_clearance_tube_baffle = value; OnPropertyChanged(nameof(diametral_clearance_tube_baffle)); } }
        public DateTime? createdAt { get => _createdAt; set { _createdAt = value; OnPropertyChanged(nameof(createdAt)); } }
        public DateTime? updatedAt { get => _updatedAt; set { _updatedAt = value; OnPropertyChanged(nameof(updatedAt)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "", bool uncheck = true)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                Task.Run(() =>
                {
                    if (uncheck)
                    {
                        GlobalFunctionsAndCallersService.Uncheck(new System.Collections.Generic.List<string>() { nameof(GeometryPage), nameof(BafflesPage) });
                    }
                    Check();
                });
            }
        }

        private void Check()
        {
            var inneDiamTubes = StringToDoubleChecker.ConvertToDouble(_inner_diameter_tubes_side);
            var inneDiamShell = StringToDoubleChecker.ConvertToDouble(_inner_diameter_shell_side);
            var numberOfTubes = StringToDoubleChecker.ConvertToDouble(_number_of_tubes);
            var numberOfPasses = StringToDoubleChecker.ConvertToDouble(_tube_plate_layout_number_of_passes);
            var tubeInnerLenght =  StringToDoubleChecker.ConvertToDouble(_tube_inner_length);
            var routhnessTubes = StringToDoubleChecker.ConvertToDouble(_roughness_tubes_side);
            var routhnessShell = StringToDoubleChecker.ConvertToDouble(_roughness_shell_side);
            var inInnerDiam = StringToDoubleChecker.ConvertToDouble(_inner_diameter_inner_side);
            var innerLength = StringToDoubleChecker.ConvertToDouble(_tube_inner_length);
            var outerDiamInner = StringToDoubleChecker.ConvertToDouble(_outer_diameter_inner_side);
            var outLengthTubes = StringToDoubleChecker.ConvertToDouble(_nozzles_out_length_tubes_side);
            var outLengthShell = StringToDoubleChecker.ConvertToDouble(_nozzles_out_length_shell_side);
            var numberOfParallelLinesTubes = StringToDoubleChecker.ConvertToDouble(_nozzles_number_of_parallel_lines_tubes_side);
            var numberOfParallelLinesShell = StringToDoubleChecker.ConvertToDouble(_nozzles_number_of_parallel_lines_shell_side);
            var minTubeHole = StringToDoubleChecker.ConvertToDouble(_clearances_spacing_minimum_tube_hole_to_tubeplate_edge);
            var tubePitch = StringToDoubleChecker.ConvertToDouble(_tube_plate_layout_tube_pitch);
            var divPlateThinkness = StringToDoubleChecker.ConvertToDouble(_tube_plate_layout_div_plate_thickness);
            var tubePlateThinkness = StringToDoubleChecker.ConvertToDouble(_tube_plate_layout_tubeplate_thickness);
            if (numberOfTubes<=0)
            {
                numberOfTubes = 12;
                _number_of_tubes = "12";
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(number_of_tubes)));
            }
            if (inneDiamTubes<=0|| inneDiamShell<=0||
                numberOfTubes<numberOfPasses||
                tubeInnerLenght<500||
                routhnessTubes <= 0|| routhnessShell<=0||
                inInnerDiam<=0||innerLength<=0||
                outerDiamInner<=0||
                outLengthTubes<=0|| outLengthShell<=0||
                numberOfParallelLinesTubes<=0|| numberOfParallelLinesShell<=0||
                minTubeHole<3||
                numberOfPasses<=0||
                divPlateThinkness<=0||tubePlateThinkness<=0||
                (_head_exchange_type!= "annular_space"&&outerDiamInner*1.25>tubePitch)
                )
            {
                GlobalFunctionsAndCallersService.SetIncorrect(new System.Collections.Generic.List<string>() { nameof(GeometryPage) });
                return;
            }
            var type = typeof(GeometryFull);
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (double.TryParse(prop.GetValue(this)?.ToString(),out var val))
                {
                    if (val<0)
                    {
                        GlobalFunctionsAndCallersService.SetIncorrect(new System.Collections.Generic.List<string>() { nameof(GeometryPage) });
                        return;
                    }
                }
            }
        }
    }
}