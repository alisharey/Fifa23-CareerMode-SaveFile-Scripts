using System;
using System.Collections.Generic;
using System.Text;

namespace FIFA23
{
    namespace Scripts
    {
        class SystemDefinitions
        {
            // Windows Specific Constants
            public const long NULL = 0;

            // Error List - AMB7500/AMB7600 Platforms Start with 0xAE100XXX
            public const long No_Error = 0x0;

            // 0xAE100000 ~ 0xAE100099		: Generic Errors
            public const long Operation_Halted_By_User = 0xAE100000;
            public const long Prerequisite_Not_Met = 0xAE100001;
            public const long Illegal_Operation = 0xAE100002;
            public const long No_Calibration_Record = 0xAE100003;
            public const long Calibration_Due = 0xAE100004;
            public const long Invalid_User_Input = 0xAE100005;
            public const long Invalid_Query = 0xAE100006;
            public const long Station_ID_Occupied = 0xAE100007;
            public const long Modulation_Overloaded = 0xAE100008;
            public const long Modulation_Not_Loaded = 0xAE100009;
            public const long Modulation_Already_Loaded = 0xAE100010;
            public const long Resource_Already_Granted = 0xAE100011;
            public const long Correlation_Error = 0xAE100012;
            public const long Invalid_Setting = 0xAE100013;
            public const long Calibration_Out_of_Specs = 0xAE100014;
            public const long Unsupported_Offline = 0xAE100015;
            public const long Enforced_Offline = 0xAE100016;
            public const long Software_Timeout = 0xAE100020;
            public const long Hardware_Timeout = 0xAE100021;
            public const long Exceed_Max_Sample_Size = 0xAE100022;
            public const long Missing_Calibration_File = 0xAE100023;
            public const long No_TestHead = 0xAE100024;
            public const long No_TestSite = 0xAE100025;
            public const long API_Not_Supported = 0xAE100026;
            public const long Offline_Mode = 0xAE100027;

            // 0xAE100100 ~ 0xAE100199		: Software Specific Errors
            public const long Load_DLL_File_Error = 0xAE100100;
            public const long Load_DLL_Function_Error = 0xAE100101;
            public const long Memory_Allocation_Error = 0xAE100102;
            public const long File_IO_Error = 0xAE100110;
            public const long Missing_Global_Cond = 0xAE100120;
            public const long Missing_FlowItem_Cond = 0xAE100121;
            public const long Missing_TestParam_Cond = 0xAE100122;
            public const long Invalid_Command = 0xAE100130;
            public const long Invalid_Command_Arguments = 0xAE100131;

            // 0xAE100200 ~ 0xAE100299		: Hardware Specific Errors
            public const long Register_Instrument_Error = 0xAE100200;
            public const long Init_Instrument_Error = 0xAE100201;
            public const long Instrument_Read_Error = 0xAE100202;
            public const long Instrument_Write_Error = 0xAE100203;
            public const long Incompatible_Hardware = 0xAE100204;
            public const long Uninit_Instrument_Error = 0xAE100205;
            public const long Compliance_Status_Error = 0xAE100206;
            public const long Voltage_Out_Of_Range = 0xAE100210;
            public const long Current_Out_Of_Range = 0xAE100211;
            public const long Invalid_Resource_Pin = 0xAE100212;
        }
    }
}
