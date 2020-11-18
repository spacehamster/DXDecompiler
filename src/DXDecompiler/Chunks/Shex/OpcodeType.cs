namespace DXDecompiler.Chunks.Shex
{
	public enum OpcodeType
	{
		[NumberType(NumberType.Float)]
		[Description("add")]
		Add,

		[NumberType(NumberType.UInt)]
		[Description("and")]
		And,

		[Description("break")]
		Break,

		[Description("breakc")]
		BreakC,

		[Description("call")]
		Call,

		[Description("callc")]
		CallC,

		[Description("case")]
		Case,

		[Description("continue")]
		Continue,

		[Description("continuec")]
		ContinueC,

		[Description("cut")]
		Cut,

		[Description("default")]
		Default,

		[Description("deriv_rtx")]
		DerivRtx,

		[Description("deriv_rty")]
		DerivRty,

		[Description("discard")]
		Discard,

		[NumberType(NumberType.Float)]
		[Description("div")]
		Div,

		[NumberType(NumberType.Float)]
		[Description("dp2")]
		Dp2,

		[NumberType(NumberType.Float)]
		[Description("dp3")]
		Dp3,

		[NumberType(NumberType.Float)]
		[Description("dp4")]
		Dp4,

		[Description("else")]
		Else,

		[Description("emit")]
		Emit,

		[Description("emitThenCut")]
		EmitThenCut,

		[Description("endif")]
		EndIf,

		[Description("endloop")]
		EndLoop,

		[Description("endswitch")]
		EndSwitch,

		[NumberType(NumberType.Float)]
		[Description("eq")]
		Eq,

		[NumberType(NumberType.Float)]
		[Description("exp")]
		Exp,

		[NumberType(NumberType.Float)]
		[Description("frc")]
		Frc,

		[Description("ftoi")]
		FtoI,

		[Description("ftou")]
		FtoU,

		[NumberType(NumberType.Float)]
		[Description("ge")]
		Ge,

		[NumberType(NumberType.Int)]
		[Description("iadd")]
		IAdd,

		[Description("if")]
		If,

		[NumberType(NumberType.Int)]
		[Description("ieq")]
		IEq,

		[NumberType(NumberType.Int)]
		[Description("ige")]
		IGe,

		[NumberType(NumberType.Int)]
		[Description("ilt")]
		ILt,

		[NumberType(NumberType.Int)]
		[Description("imad")]
		IMad,

		[NumberType(NumberType.Int)]
		[Description("imax")]
		IMax,

		[NumberType(NumberType.Int)]
		[Description("imin")]
		IMin,

		[NumberType(NumberType.Int)]
		[Description("imul")]
		IMul,

		[NumberType(NumberType.Int)]
		[Description("ine")]
		INe,

		[NumberType(NumberType.Int)]
		[Description("ineg")]
		INeg,

		[NumberType(NumberType.Int)]
		[Description("ishl")]
		IShl,

		[NumberType(NumberType.Int)]
		[Description("ishr")]
		IShr,

		[Description("itof")]
		ItoF,

		[Description("label")]
		Label,

		[NumberType(NumberType.UInt)]
		[Description("ld")]
		Ld,

		[NumberType(NumberType.UInt)]
		[Description("ldms")]
		LdMs,

		[NumberType(NumberType.Float)]
		[Description("log")]
		Log,

		[Description("loop")]
		Loop,

		[NumberType(NumberType.Float)]
		[Description("lt")]
		Lt,

		[NumberType(NumberType.Float)]
		[Description("mad")]
		Mad,

		[NumberType(NumberType.Float)]
		[Description("min")]
		Min,

		[NumberType(NumberType.Float)]
		[Description("max")]
		Max,

		CustomData,

		[Description("mov")]
		Mov,

		[Description("movc")]
		MovC,

		[NumberType(NumberType.Float)]
		[Description("mul")]
		Mul,

		[NumberType(NumberType.Float)]
		[Description("ne")]
		Ne,

		[Description("nop")]
		Nop,

		[Description("not")]
		Not,

		[NumberType(NumberType.UInt)]
		[Description("or")]
		Or,

		[Description("resinfo")]
		Resinfo,

		[Description("ret")]
		Ret,

		[Description("retc")]
		RetC,

		[NumberType(NumberType.Float)]
		[Description("round_ne")]
		RoundNe,

		[NumberType(NumberType.Float)]
		[Description("round_ni")]
		RoundNi,

		[NumberType(NumberType.Float)]
		[Description("round_pi")]
		RoundPi,

		[NumberType(NumberType.Float)]
		[Description("round_z")]
		RoundZ,

		[NumberType(NumberType.Float)]
		[Description("rsq")]
		Rsq,

		[NumberType(NumberType.Float)]
		[Description("sample")]
		Sample,

		[NumberType(NumberType.Float)]
		[Description("sample_c")]
		SampleC,

		[NumberType(NumberType.Float)]
		[Description("sample_c_lz")]
		SampleCLz,

		[NumberType(NumberType.Float)]
		[Description("sample_l")]
		SampleL,

		[NumberType(NumberType.Float)]
		[Description("sample_d")]
		SampleD,

		[NumberType(NumberType.Float)]
		[Description("sample_b")]
		SampleB,

		[NumberType(NumberType.Float)]
		[Description("sqrt")]
		Sqrt,

		[NumberType(NumberType.Float)]
		[Description("switch")]
		Switch,

		[NumberType(NumberType.Float)]
		[Description("sincos")]
		Sincos,

		[NumberType(NumberType.UInt)]
		[Description("udiv")]
		UDiv,

		[NumberType(NumberType.UInt)]
		[Description("ult")]
		ULt,

		[NumberType(NumberType.UInt)]
		[Description("uge")]
		UGe,

		[NumberType(NumberType.UInt)]
		[Description("umul")]
		UMul,

		[NumberType(NumberType.UInt)]
		[Description("umad")]
		UMad,

		[NumberType(NumberType.UInt)]
		[Description("umax")]
		UMax,

		[NumberType(NumberType.UInt)]
		[Description("umin")]
		UMin,

		[NumberType(NumberType.UInt)]
		[Description("ushr")]
		UShr,

		[Description("utof")]
		Utof,

		[NumberType(NumberType.UInt)]
		[Description("xor")]
		Xor,

		[Description("dcl_resource")]
		DclResource,

		[Description("dcl_constantbuffer")]
		DclConstantBuffer,

		[Description("dcl_sampler")]
		DclSampler,

		[Description("dcl_indexrange")]
		DclIndexRange,

		[Description("dcl_outputtopology")]
		DclGsOutputPrimitiveTopology,

		[Description("dcl_inputprimitive")]
		DclGsInputPrimitive,

		[Description("dcl_maxout")]
		DclMaxOutputVertexCount,

		[Description("dcl_input")]
		DclInput,

		[Description("dcl_input_sgv")]
		DclInputSgv,

		[Description("dcl_input_siv")]
		DclInputSiv,

		[Description("dcl_input_ps")]
		DclInputPs,

		[Description("dcl_input_ps_sgv")]
		DclInputPsSgv,

		[Description("dcl_input_ps_siv")]
		DclInputPsSiv,

		[Description("dcl_output")]
		DclOutput,

		[Description("dcl_output_sgv")]
		DclOutputSgv,

		[Description("dcl_output_siv")]
		DclOutputSiv,

		[Description("dcl_temps")]
		DclTemps,

		[Description("dcl_indexableTemp")]
		DclIndexableTemp,

		[Description("dcl_globalFlags")]
		DclGlobalFlags,

		/// <summary>
		/// This marks the end of D3D10.0 opcodes
		/// </summary>
		D3D10Count,

		// DX 10.1 op codes

		[Description("lod")]
		Lod,

		[Description("gather4")]
		Gather4,

		[Description("samplepos")]
		SamplePos,

		[Description("sampleinfo")]
		SampleInfo,

		/// <summary>
		/// This marks the end of D3D10.1 opcodes
		/// </summary>
		D3D10_1Count,

		// DX 11 op codes

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_decls")]
		HsDecls,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_control_point_phase")]
		HsControlPointPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_fork_phase")]
		HsForkPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_join_phase")]
		HsJoinPhase,

		[Description("emit_stream")]
		EmitStream,

		[Description("cut_stream")]
		CutStream,

		// TODO: Finish descriptions.
		EmitThenCutStream,

		[Description("fcall")]
		InterfaceCall,

		[Description("bufinfo")]
		Bufinfo,

		[Description("deriv_rtx_coarse")]
		RtxCoarse,

		[Description("deriv_rtx_fine")]
		RtxFine,

		[Description("deriv_rty_coarse")]
		RtyCoarse,

		[Description("deriv_rty_fine")]
		RtyFine,

		[Description("gather4_c")]
		Gather4C,

		[Description("gather4_po")]
		Gather4Po,

		[Description("gather4_po_c")]
		Gather4PoC,

		[Description("rcp")]
		Rcp,

		[NumberType(NumberType.Float)]
		F32ToF16,

		[NumberType(NumberType.Float)]
		F16ToF32,

		[NumberType(NumberType.UInt)]
		UAddC,

		[NumberType(NumberType.UInt)]
		USubB,

		[NumberType(NumberType.UInt)]
		[Description("countbits")]
		CountBits,

		[NumberType(NumberType.UInt)]
		[Description("firstbit_hi")]
		FirstBitHi,

		[NumberType(NumberType.UInt)]
		[Description("firstbit_lo")]
		FirstBitLo,

		[NumberType(NumberType.UInt)]
		[Description("firstbit_shi")]
		FirstBitSHi,

		[NumberType(NumberType.UInt)]
		[Description("ubfe")]
		UBfe,

		[NumberType(NumberType.Int)]
		[Description("ibfe")]
		IBfe,

		[NumberType(NumberType.UInt)]
		[Description("bfi")]
		Bfi,

		[Description("bfrev")]
		BfRev,

		[Description("swapc")]
		SwapC,

		[Description("dcl_stream")]
		DclStream,

		[Description("dcl_function_body")]
		DclFunctionBody,

		[Description("dcl_function_table")]
		DclFunctionTable,

		[Description("dcl_interface")]
		DclInterface,

		[Description("dcl_input_control_point_count")]
		DclInputControlPointCount,

		[Description("dcl_output_control_point_count")]
		DclOutputControlPointCount,

		[Description("dcl_tessellator_domain")]
		DclTessDomain,

		[Description("dcl_tessellator_partitioning")]
		DclTessPartitioning,

		[Description("dcl_tessellator_output_primitive")]
		DclTessOutputPrimitive,

		[Description("dcl_hs_max_tessfactor")]
		DclHsMaxTessFactor,

		[Description("dcl_hs_fork_phase_instance_count")]
		DclHsForkPhaseInstanceCount,

		[Description("dcl_hs_join_phase_instance_count")]
		DclHsJoinPhaseInstanceCount,

		[Description("dcl_thread_group")]
		DclThreadGroup,

		[Description("dcl_uav_typed")]
		DclUnorderedAccessViewTyped,

		[Description("dcl_uav_raw")]
		DclUnorderedAccessViewRaw,

		[Description("dcl_uav_structured")]
		DclUnorderedAccessViewStructured,

		[Description("dcl_tgsm_raw")]
		DclThreadGroupSharedMemoryRaw,

		[Description("dcl_tgsm_structured")]
		DclThreadGroupSharedMemoryStructured,

		[Description("dcl_resource_raw")]
		DclResourceRaw,

		[Description("dcl_resource_structured")]
		DclResourceStructured,

		[Description("ld_uav_typed")]
		LdUavTyped,

		[Description("store_uav_typed")]
		StoreUavTyped,

		[Description("ld_raw")]
		LdRaw,

		[Description("store_raw")]
		StoreRaw,

		[Description("ld_structured")]
		LdStructured,

		[Description("store_structured")]
		StoreStructured,

		[NumberType(NumberType.UInt)]
		[Description("atomic_and")]
		AtomicAnd,

		[NumberType(NumberType.UInt)]
		[Description("atomic_or")]
		AtomicOr,

		[NumberType(NumberType.UInt)]
		[Description("atomic_xor")]
		AtomicXor,

		[NumberType(NumberType.UInt)]
		[Description("atomic_cmp_store")]
		AtomicCmpStore,

		[NumberType(NumberType.Int)]
		[Description("atomic_iadd")]
		AtomicIAdd,

		[NumberType(NumberType.Int)]
		[Description("atomic_imax")]
		AtomicIMax,

		[NumberType(NumberType.Int)]
		[Description("atomic_imin")]
		AtomicIMin,

		[NumberType(NumberType.UInt)]
		[Description("atomic_umax")]
		AtomicUMax,

		[NumberType(NumberType.UInt)]
		[Description("atomic_umin")]
		AtomicUMin,

		[Description("imm_atomic_alloc")]
		ImmAtomicAlloc,

		[Description("imm_atomic_consume")]
		ImmAtomicConsume,

		[NumberType(NumberType.Int)]
		[Description("imm_atomic_iadd")]
		ImmAtomicIAdd,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_and")]
		ImmAtomicAnd,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_or")]
		ImmAtomicOr,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_xor")]
		ImmAtomicXor,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_exch")]
		ImmAtomicExch,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_cmp_exch")]
		ImmAtomicCmpExch,

		[NumberType(NumberType.Int)]
		[Description("imm_atomic_imax")]
		ImmAtomicIMax,

		[NumberType(NumberType.Int)]
		[Description("imm_atomic_imin")]
		ImmAtomicIMin,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_umax")]
		ImmAtomicUMax,

		[NumberType(NumberType.UInt)]
		[Description("imm_atomic_umin")]
		ImmAtomicUMin,

		[Description("sync")]
		Sync,

		[NumberType(NumberType.Double)]
		[Description("dadd")]
		DAdd,

		[NumberType(NumberType.Double)]
		[Description("dmax")]
		DMax,

		[NumberType(NumberType.Double)]
		[Description("dmin")]
		DMin,

		[NumberType(NumberType.Double)]
		[Description("dmul")]
		DMul,

		[NumberType(NumberType.Double)]
		[Description("deq")]
		DEq,

		[NumberType(NumberType.Double)]
		[Description("dge")]
		DGe,

		[NumberType(NumberType.Double)]
		[Description("dlt")]
		DLt,

		[NumberType(NumberType.Double)]
		[Description("dne")]
		DNe,

		[NumberType(NumberType.Double)]
		[Description("dmov")]
		DMov,

		[NumberType(NumberType.Double)]
		[Description("dmovc")]
		DMovC,

		[Description("dtof")]
		DToF,
		[Description("ftod")]
		FToD,
		[Description("eval_snapped")]
		EvalSnapped,
		[Description("eval_sample_index")]
		EvalSampleIndex,
		[Description("eval_centroid")]
		EvalCentroid,
		[Description("dcl_gsinstances")]
		DclGsInstanceCount,

		[Description("abort")]
		Abort,

		DebugBreak,

		/// <summary>
		/// This marks the end of D3D11.0 opcodes
		/// </summary>
		D3D11_0Count,

		[Description("ddiv")]
		Ddiv,
		[Description("dfma")]
		Dfma,
		[Description("drcp")]
		Drcp,

		[Description("msad")]
		Msad,

		[Description("dtoi")]
		Dtoi,
		[Description("dtou")]
		Dtou,
		[Description("itod")]
		Itod,
		[Description("utod")]
		Utod,

		/// <summary>
		/// This marks the end of D3D11.1 opcodes
		/// </summary>
		D3D11_1Count,

		[Description("gather4_s")]
		Gather4S,

		[Description("gather4_c_s")]
		Gather4CS,

		[Description("gather4_po_s")]
		Gather4PoS,

		[Description("gather4_po_c_s")]
		Gather4PoCS,

		[Description("ld_s")]
		LdS,

		[Description("ldms_s")]
		LdMsS,

		[Description("ld_uav_typed_s")]
		LdUavTypedS,

		[Description("ld_raw_s")]
		LdRawS,

		[Description("ld_structured_s")]
		LdStructuredS,

		[Description("sample_l_s")]
		SampleLS,

		[Description("sample_c_lz_s")]
		SampleCLzS,

		[Description("sample_cl_s")]
		SampleClS,

		[Description("sample_b_cl_s")]
		SampleBClS,

		[Description("sample_d_cl_s")]
		SampleDClS,

		[Description("sample_c_cl_s")]
		SampleCClS,

		[Description("check_access_fully_mapped")]
		CheckAccessFullyMapped,

		/// <summary>
		/// This marks the end of WDDM 1.3 opcodes
		/// </summary>
		D3DWDDM1_3Count,
	}
}