//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
//
// Buffer Definitions: 
//
// cbuffer $Globals
// {
//
//   float4 gTest;                      // Offset:    0 Size:    16
//
// }
//
//
// Resource Bindings:
//
// Name                                 Type  Format         Dim      HLSL Bind  Count
// ------------------------------ ---------- ------- ----------- -------------- ------
// $Globals                          cbuffer      NA          NA            cb0      1 
//
//
//
// Patch Constant signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_TessFactor            0   x           0 QUADEDGE   float   x   
// SV_TessFactor            1   x           1 QUADEDGE   float   x   
// SV_TessFactor            2   x           2 QUADEDGE   float   x   
// SV_TessFactor            3   x           3 QUADEDGE   float   x   
// SV_InsideTessFactor      0   x           4  QUADINT   float   x   
// SV_InsideTessFactor      1   x           5  QUADINT   float   x   
// TANGENT                  0   xyz         6     NONE   float   xyz 
// TANGENT                  1   xyz         7     NONE   float   xyz 
// TANGENT                  2   xyz         8     NONE   float   xyz 
// TANGENT                  3   xyz         9     NONE   float   xyz 
// TEXCOORD                 0   xy         10     NONE   float   xy  
// TEXCOORD                 1   xy         11     NONE   float   xy  
// TEXCOORD                 2   xy         12     NONE   float   xy  
// TEXCOORD                 3   xy         13     NONE   float   xy  
// TANUCORNER               0   xyz        14     NONE   float   xyz 
// TANUCORNER               1   xyz        15     NONE   float   xyz 
// TANUCORNER               2   xyz        16     NONE   float   xyz 
// TANUCORNER               3   xyz        17     NONE   float   xyz 
// TANVCORNER               0   xyz        18     NONE   float   xyz 
// TANVCORNER               1   xyz        19     NONE   float   xyz 
// TANVCORNER               2   xyz        20     NONE   float   xyz 
// TANVCORNER               3   xyz        21     NONE   float   xyz 
// TANWEIGHTS               0   xyzw       22     NONE   float   xyzw
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// WORLDPOS                 0   xyz         0     NONE   float   xyz 
// TEXCOORD                 0   xy          1     NONE   float   xy  
// TANGENT                  0   xyz         2     NONE   float       
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// BEZIERPOS                0   xyz         0     NONE   float   xyz 
//
// Tessellation Domain   # of control points
// -------------------- --------------------
// Quadrilateral                          16
//
// Tessellation Output Primitive  Partitioning Type 
// ------------------------------ ------------------
// Clockwise Triangles            Integer           
//
hs_5_0
hs_decls 
dcl_input_control_point_count 32
dcl_output_control_point_count 16
dcl_tessellator_domain domain_quad
dcl_tessellator_partitioning partitioning_integer
dcl_tessellator_output_primitive output_triangle_cw
dcl_hs_max_tessfactor l(64.000000)
dcl_globalFlags refactoringAllowed
dcl_constantbuffer CB0[1], immediateIndexed
hs_control_point_phase 
dcl_input vOutputControlPointID
dcl_input vPrim
dcl_input v[32][0].x
dcl_input v[32][1].xy
dcl_output o0.xyz
dcl_temps 1
utof r0.x, vPrim
mov r0.y, vOutputControlPointID
mul o0.xy, r0.xxxx, v[r0.y + 0][1].xyxx
utof r0.x, vOutputControlPointID
mul o0.z, r0.x, v[2][0].x
ret 
hs_fork_phase 
dcl_input vocp[16][0].x
dcl_output_siv o0.x, finalQuadUeq0EdgeTessFactor
min o0.x, l(64.000000), vocp[0][0].x
ret 
hs_fork_phase 
dcl_input vicp[32][0].y
dcl_output_siv o1.x, finalQuadVeq0EdgeTessFactor
min o1.x, l(64.000000), vicp[0][0].y
ret 
hs_fork_phase 
dcl_input vicp[32][0].z
dcl_input vocp[16][0].z
dcl_output_siv o2.x, finalQuadUeq1EdgeTessFactor
dcl_temps 1
mul r0.x, vicp[0][0].z, vocp[0][0].z
min o2.x, r0.x, l(64.000000)
ret 
hs_fork_phase 
dcl_output_siv o3.x, finalQuadVeq1EdgeTessFactor
min o3.x, cb0[0].w, l(64.000000)
ret 
hs_fork_phase 
dcl_hs_fork_phase_instance_count 2
dcl_input vForkInstanceID
dcl_output_siv o4.x, finalQuadUInsideTessFactor
dcl_output_siv o5.x, finalQuadVInsideTessFactor
dcl_temps 1
dcl_indexrange o4.x 2
mov r0.x, vForkInstanceID.x
mov o[r0.x + 4].x, l(0)
ret 
hs_fork_phase 
dcl_input vocp[16][0].x
dcl_output o6.x
mov o6.x, vocp[0][0].x
ret 
hs_fork_phase 
dcl_input vocp[16][0].y
dcl_output o6.y
mov o6.y, vocp[0][0].y
ret 
hs_fork_phase 
dcl_input vocp[16][0].z
dcl_output o6.z
mov o6.z, vocp[0][0].z
ret 
hs_fork_phase 
dcl_input vicp[32][0].x
dcl_output o7.x
mov o7.x, vicp[0][0].x
ret 
hs_fork_phase 
dcl_input vicp[32][0].y
dcl_output o7.y
mov o7.y, vicp[0][0].y
ret 
hs_fork_phase 
dcl_input vicp[32][0].z
dcl_output o7.z
mov o7.z, vicp[0][0].z
ret 
hs_fork_phase 
dcl_input vicp[32][0].x
dcl_input vocp[16][0].x
dcl_output o8.x
mul o8.x, vicp[0][0].x, vocp[0][0].x
ret 
hs_fork_phase 
dcl_input vicp[32][0].y
dcl_input vocp[16][0].y
dcl_output o8.y
mul o8.y, vicp[0][0].y, vocp[0][0].y
ret 
hs_fork_phase 
dcl_input vicp[32][0].z
dcl_input vocp[16][0].z
dcl_output o8.z
mul o8.z, vicp[0][0].z, vocp[0][0].z
ret 
hs_fork_phase 
dcl_output o9.x
mov o9.x, cb0[0].x
ret 
hs_fork_phase 
dcl_output o9.y
mov o9.y, cb0[0].y
ret 
hs_fork_phase 
dcl_output o9.z
mov o9.z, cb0[0].z
ret 
hs_fork_phase 
dcl_input vPrim
dcl_input vocp[16][0].x
dcl_output o10.x
dcl_temps 1
mov r0.x, vPrim
mov o10.x, vocp[r0.x + 0][0].x
ret 
hs_fork_phase 
dcl_input vPrim
dcl_input vocp[16][0].y
dcl_output o10.y
dcl_temps 1
mov r0.x, vPrim
mov o10.y, vocp[r0.x + 0][0].y
ret 
hs_fork_phase 
dcl_input vPrim
dcl_input vicp[32][0].x
dcl_output o11.x
dcl_temps 1
mov r0.x, vPrim
mov o11.x, vicp[r0.x + 0][0].x
ret 
hs_fork_phase 
dcl_input vPrim
dcl_input vicp[32][0].y
dcl_output o11.y
dcl_temps 1
mov r0.x, vPrim
mov o11.y, vicp[r0.x + 0][0].y
ret 
hs_fork_phase 
dcl_input vPrim
dcl_input vicp[32][0].x
dcl_input vocp[16][0].x
dcl_output o12.x
dcl_temps 1
mov r0.x, vPrim
mul o12.x, vicp[r0.x + 0][0].x, vocp[r0.x + 0][0].x
ret 
hs_fork_phase 
dcl_input vPrim
dcl_input vicp[32][0].y
dcl_input vocp[16][0].y
dcl_output o12.y
dcl_temps 1
mov r0.x, vPrim
mul o12.y, vicp[r0.x + 0][0].y, vocp[r0.x + 0][0].y
ret 
hs_fork_phase 
dcl_input vPrim
dcl_output o13.x
utof o13.x, vPrim
ret 
hs_fork_phase 
dcl_input vPrim
dcl_output o13.y
utof o13.y, vPrim
ret 
hs_fork_phase 
dcl_hs_fork_phase_instance_count 9
dcl_input vForkInstanceID
dcl_output o14.x
dcl_output o15.x
dcl_output o16.x
dcl_output o17.x
dcl_output o18.x
dcl_output o19.x
dcl_output o20.x
dcl_output o21.x
dcl_output o22.x
dcl_temps 1
dcl_indexrange o14.x 9
mov r0.x, vForkInstanceID.x
mov o[r0.x + 14].x, l(0)
ret 
hs_fork_phase 
dcl_hs_fork_phase_instance_count 9
dcl_input vForkInstanceID
dcl_output o14.y
dcl_output o15.y
dcl_output o16.y
dcl_output o17.y
dcl_output o18.y
dcl_output o19.y
dcl_output o20.y
dcl_output o21.y
dcl_output o22.y
dcl_temps 1
dcl_indexrange o14.y 9
mov r0.x, vForkInstanceID.x
mov o[r0.x + 14].y, l(0)
ret 
hs_fork_phase 
dcl_hs_fork_phase_instance_count 9
dcl_input vForkInstanceID
dcl_output o14.z
dcl_output o15.z
dcl_output o16.z
dcl_output o17.z
dcl_output o18.z
dcl_output o19.z
dcl_output o20.z
dcl_output o21.z
dcl_output o22.z
dcl_temps 1
dcl_indexrange o14.z 9
mov r0.x, vForkInstanceID.x
mov o[r0.x + 14].z, l(0)
ret 
hs_fork_phase 
dcl_output o22.w
mov o22.w, l(0)
ret 
// Approximately 75 instruction slots used
