#pragma FXC LightingVS_FX fx_2_0
//
// Shaders used by the LightingVS D3D sample 
//
// Note: This effect file does not work with EffectEdit.
//

// This shader transforms input vertices to the projection space and
// computes lighting in the camera space for different light types.
// Specular color is not computed, because the sample disables specular lighting.
//
// Constant allocation
//
// c0   - material ambient color 
// c1   - material emissive color 
// c2   - material diffuse color 
// c3   - material specular color 
// c4.x - material specular power 
//
// Each light is described by 7 vectors:
//
// c5  - light ambient color multiplied by material ambient color
// c6  - light diffuse color 
// c7  - light specular color 
// c8  - light direction in camera space (normalized vector)
// c9  - light position in camera space  
// c10 - light attenuation factors (att0, att1, att2, lightType) 
// c11 - light parameters (falloff, -cos(phi/2), 1/(cos(theta/2) - cos(phi/2)), 0) 
// 
// c12 -  2 light 
// c19 -  3 light 
// c26 -  4 light 
// c33 -  5 light 
// c40 -  6 light 
// c47 -  7 light 
// c54 -  8 light 
// c61 -  9 light 
// c67 - 10 light 
// 
// c100 - c103   - transformation matrix to the projection space 
// c104 - c107   - transformation matrix to the camera space 
// c108 - c111   - inverse transposed of transformation matrix to the camera space 
// 
// c112  - (0,1,2,3) 
// c113  - initial diffuse color (emissive + material_ambient * global_ambient) 
// 
// i0            - (number of directional lights, StartIndex, 7, 0) 
// i1            - (number of spot lights, StartIndex, 7, 0) 
// i2            - (number of point lights, StartIndex, 7, 0) 
// 

#define MaterialAmbient                c0 
#define MaterialEmissive               c1 
#define MaterialDiffuse                c2 
#define MaterialSpecular               c3  
#define MaterialSpecularPower          c4.x 

#define GlobalAmbientFactor            c113 

#define Mproj                          c100 
#define Mcamera                        c104 
#define Mcamera_inverse_transposed     c108 

#define VertexPosition                 v0 
#define VertexNormal                   v1 

#define LightAmbient                   c5[aL] 
#define LightDiffuse                   c6[aL] 
#define LightSpecular                  c7[aL] 
#define LightDirection                 c8[aL]
#define LightPosition                  c9[aL] 
#define LightAttenuation0              c10[aL].x 
#define LightAttenuation1              c10[aL].y 
#define LightAttenuation2              c10[aL].z 
#define LightFallof                    c11[aL].x 
#define LightSpotFactor1               c11[aL].y 
#define LightSpotFactor2               c11[aL].z 

#define Zero                           c112.x 
#define One                            c112.y 

#define VertexPositionCameraSpace      r0.xyz
#define VertexNormalCameraSpace        r1.xyz
#define OutputDiffuse                  r2 
#define TempColor                      r4   
#define LightToVertexVector            r5 
#define AttenuationFactor              r6.x 
#define SpotFactor                     r6.y 
#define Rho                            r6.y 
#define LightVertexDistance            r6.z 
#define DotProduct                     r7.x 

VertexShader Lighting_VS_2_0 = asm 
{ 
    vs_2_0 

    ; input declaration 
    dcl_position0 VertexPosition 
    dcl_normal0   VertexNormal 

    ;transform position to the projection space 
    m4x4 oPos, VertexPosition, Mproj

    ;initialize output diffuse color
    mov OutputDiffuse, GlobalAmbientFactor

    ;transform position to the camera space 
    m4x3 VertexPositionCameraSpace, VertexPosition, Mcamera

    ;transform normal to the camera space 
    ;assume that the transformation matrix is orthogonal, so we do not normalize 
    ;normals after transformation
    m3x3 VertexNormalCameraSpace, VertexNormal, Mcamera_inverse_transposed

    // Loop through directional lights 

    loop aL, i0 
        mov LightToVertexVector, LightDirection     // Vector from light to vertex
        mov AttenuationFactor, One                  // att * spot = 1.0f
        call l0 
    endloop 

    // Loop through spot lights 

    loop aL, i1 
        ;compute distance to the light  and  
        ;normalized vector from light to a vertex  
        add LightToVertexVector, VertexPositionCameraSpace, -LightPosition 
        dp3 LightVertexDistance, LightToVertexVector, LightToVertexVector
        rsq LightVertexDistance, LightVertexDistance       
        mul LightToVertexVector, LightToVertexVector, LightVertexDistance   
        rcp LightVertexDistance, LightVertexDistance 

        ; compute attenuation factor 
        mad AttenuationFactor, LightAttenuation2, LightVertexDistance, LightAttenuation1 
        mad AttenuationFactor, AttenuationFactor, LightVertexDistance, LightAttenuation0 
        rcp AttenuationFactor, AttenuationFactor 

        ; compute spot factor 
        dp3 Rho, LightToVertexVector, LightDirection 
        add SpotFactor, Rho, LightSpotFactor1              ; Rho - cos(phi/2) 
        max SpotFactor, SpotFactor, Zero                   ; max((Rho - cos(phi/2), 0) 
        mul SpotFactor, SpotFactor, LightSpotFactor2 
        pow SpotFactor, SpotFactor, LightFallof      

        ;combined att*spot 
        mul AttenuationFactor, AttenuationFactor, SpotFactor 
        call l0 
    endloop 

    // Loop through point lights 

    loop aL, i2 
        ;compute distance to the light and a normalized vector from light to a vertex 
        add LightToVertexVector, VertexPositionCameraSpace, -LightPosition  
        dp3 LightVertexDistance, LightToVertexVector, LightToVertexVector
        rsq LightVertexDistance, LightVertexDistance 
        mul LightToVertexVector, LightToVertexVector, LightVertexDistance   
        rcp LightVertexDistance, LightVertexDistance 

        ; compute attenuation factor 
        mad AttenuationFactor, LightAttenuation2, LightVertexDistance, LightAttenuation1 
        mad AttenuationFactor, AttenuationFactor, LightVertexDistance, LightAttenuation0 
        rcp AttenuationFactor, AttenuationFactor 

        call l0 
    endloop 

    ;output the result color 
    mov oD0, OutputDiffuse 
    mov oD0.w, MaterialDiffuse.w 
    ret 

    // This function computes diffuse component and updates the result color
    // Parameters:
    //      Normal in camera space
    //      Current diffuse color (in/out)
    //      Vector from vertex to the light
    //      Attenuation factor * spot factor
    //
    label l0 
        dp3 DotProduct, VertexNormalCameraSpace, -LightToVertexVector
        max DotProduct, DotProduct, Zero 
        mul TempColor, DotProduct, LightDiffuse    
        mul TempColor, TempColor, MaterialDiffuse  
        add TempColor, TempColor, LightAmbient     
        mad OutputDiffuse, TempColor, AttenuationFactor, OutputDiffuse 
    ret
};

VertexShader Lighting_VS_3_0 = asm
{
    vs_3_0 

    ; input declaration 
    dcl_position0 VertexPosition 
    dcl_normal0   VertexNormal 

    ;output declaration
    dcl_position0 o0 
    dcl_color0    o1 


    ;transform position to the projection space 
    m4x4 o0, VertexPosition, Mproj

    ;initialize output diffuse color
    mov OutputDiffuse, GlobalAmbientFactor

    ;transform position to the camera space 
    m4x3 VertexPositionCameraSpace, VertexPosition, Mcamera

    ;transform normal to the camera space 
    ;assume that the transformation matrix is orthogonal, so we do not normalize 
    ;normals after transformation
    m3x3 VertexNormalCameraSpace, VertexNormal, Mcamera_inverse_transposed

    // Loop through directional lights 

    loop aL, i0 
        mov LightToVertexVector, LightDirection     // Vector from light to vertex
        mov AttenuationFactor, One                  // att * spot = 1.0f
        call l0 
    endloop 

    // Loop through spot lights 

    loop aL, i1 
	    add LightToVertexVector, VertexPositionCameraSpace, -LightPosition 
        dp3 DotProduct, VertexNormalCameraSpace, -LightToVertexVector
        if_gt DotProduct, Zero
		    ; compute spot factor 
		    dp3 Rho, LightToVertexVector, LightDirection 
		    if_gt Rho, Zero
			    ;compute distance to the light  and  
			    ;normalized vector from light to a vertex  
			    dp3 LightVertexDistance, LightToVertexVector, LightToVertexVector
			    rsq LightVertexDistance, LightVertexDistance       
			    mul LightToVertexVector, LightToVertexVector, LightVertexDistance   
			    ;rcp LightVertexDistance, LightVertexDistance 

			    ; compute attenuation factor 
			    ;mad AttenuationFactor, LightAttenuation2, LightVertexDistance, LightAttenuation1 
			    ;mad AttenuationFactor, AttenuationFactor, LightVertexDistance, LightAttenuation0 
			    ;rcp AttenuationFactor, AttenuationFactor 
			    mov AttenuationFactor, One

			    ; compute spot factor 
			    dp3 Rho, LightToVertexVector, LightDirection 
			    add SpotFactor, Rho, LightSpotFactor1              ; Rho - cos(phi/2) 
			    if_gt Rho, Zero
				    mul SpotFactor, SpotFactor, LightSpotFactor2 
				    ; Use low precision method to compute pow(SpotFactor, LightFallof)
				    logp TempColor.w, SpotFactor
				    mul  TempColor.w, TempColor, LightFallof 
				    expp SpotFactor, TempColor.w 

				    ;combined att*spot 
				    mul AttenuationFactor, AttenuationFactor, SpotFactor 
				    call l0 
			    endif
		    endif
	    endif
    endloop 

    // Loop through point lights 

    loop aL, i2 
        ;compute distance to the light and a normalized vector from light to a vertex 
        add LightToVertexVector, VertexPositionCameraSpace, -LightPosition  
        dp3 LightVertexDistance, LightToVertexVector, LightToVertexVector
        rsq LightVertexDistance, LightVertexDistance 
        mul LightToVertexVector, LightToVertexVector, LightVertexDistance   
        rcp LightVertexDistance, LightVertexDistance 

        ; compute attenuation factor 
        mad AttenuationFactor, LightAttenuation2, LightVertexDistance, LightAttenuation1 
        mad AttenuationFactor, AttenuationFactor, LightVertexDistance, LightAttenuation0 
        rcp AttenuationFactor, AttenuationFactor 

        call l0 
    endloop 

    ;output the result color 
    mov o1, OutputDiffuse 
    mov o1.w, MaterialDiffuse.w 
    ret 

    // This function computes diffuse component and updates the result color
    // Parameters:
    //      Normal in camera space
    //      Current diffuse color (in/out)
    //      Vector from vertex to the light
    //      Attenuation factor * spot factor
    //
    label l0 
        dp3 DotProduct, VertexNormalCameraSpace, -LightToVertexVector
        max DotProduct, DotProduct, Zero 
        mul TempColor.xyz, DotProduct, LightDiffuse    
        mul TempColor.xyz, TempColor, MaterialDiffuse  
        add TempColor.xyz, TempColor, LightAmbient     
        mad OutputDiffuse.xyz, TempColor, AttenuationFactor, OutputDiffuse 
    ret
};

PixelShader LightingPS = asm
{
    ps_3_0 
    dcl_color0 v0
    mov oC0, v0
};

technique Technique_Lighting_VS_2_0
{
	pass P0
	{
		VertexShader = (Lighting_VS_2_0);
	}
}

technique Technique_Lighting_VS_3_0
{
	pass P0
	{
		VertexShader = (Lighting_VS_3_0);
		PixelShader  = (LightingPS);
	}
}

technique Technique_Lighting_VS_3_0_No_PS
{
	pass P0
	{
		VertexShader = (Lighting_VS_3_0);
	}
}
