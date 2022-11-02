# smallEPD_image_converter

This is the repository for the smallEPD Arduino library image converter.
for use you can just download the "PL_smallEPD_image_converter_exe.zip", unpack it, and start "image_converter.exe"
for the Lectum (4 GL), the RGB color mode: black(grey level 0) <0x00,0x00,0x00>; grey level 4 <0x44,0x44,0x44>; grey level 11 <0xAA,0xAA,0xAA>; white(grey level 15) <0xFF,0xFF,0xFF> 
for the Legio, the RGB color mode: black <0,0,0>; white <0xFF,0xFF,0xFF>; red <0xFF,0x00,0x00>; green <0x00,0xFF,0x00>;  blue <0xFF,0xFF,0xFF>; yellow <0xFF,0xFF,0x00>; 
Tolerance:   1)black:  R<=0xCD && G<=0xCD && B<=0xCD;
             2)white:  R>=0xCE && G>=0xCE && B=>0xCE;
             3)red:    R>=0xAF && G<=0x50 && B<=0x50;
             4)green:  R<=0x50 && G>=0xAF && B<=0x50;
             5)blue:   R<=0x50 && G<=0x50 && B>=0xAF;
             6)yellow: R>=0xAF && G>=0xAF && B<=0x50;
