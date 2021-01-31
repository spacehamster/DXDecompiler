#pragma FXC PixelSvTargets ps_5_0 main
#pragma FXC PixelSvTargetsInnerCoverage ps_5_0 main /DInnerCoverage=1
#pragma FXC PixelSvTargetsDepthGreaterEqual ps_5_0 main /DDEPTH=1
#pragma FXC PixelSvTargetsDepthLesserEqual ps_5_0 main /DDEPTH=2

struct PS_INPUT {
    //SV_COVERAGE AND SV_InnerCoverage are mutually exclusive
#ifndef InnerCoverage
    uint svCoverage : SV_Coverage;
#else
    uint svInnerCoverage : SV_InnerCoverage;
#endif
};
struct PS_OUTPUT {
    //Only one depth output allowed
#if DEPTH == 1
    float svDepthGreaterEqual : SV_DepthGreaterEqual;
#elif DEPTH == 2
    float svDepthLessEqual : SV_DepthLessEqual;
#else
    float svDepth : SV_Depth;
#endif
};
PS_OUTPUT main(PS_INPUT input)
{
    PS_OUTPUT output = (PS_OUTPUT)0;
    return output;
}