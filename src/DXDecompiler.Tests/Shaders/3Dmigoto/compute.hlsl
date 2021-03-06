#pragma FXC compute cs_5_0 main /Od

// Compile with fxc /T cs_5_0 /Od /Fo compute.bin
// Disable optimisations to make sure all 6 sync instructions are used

struct buf {
	float foo;
	float bar;
};

RWByteAddressBuffer rw_byte_buf : register(u0);
globallycoherent RWByteAddressBuffer rw_byte_buf_glc;
globallycoherent RWStructuredBuffer<struct buf> rw_structured_glc;
globallycoherent RWBuffer<float> rw_buf_glc;
globallycoherent RWTexture1D<float> rw_tex1d_glc;
globallycoherent RWTexture1DArray<float> rw_tex1d_array_glc;
globallycoherent RWTexture2D<float> rw_tex2d_glc;
globallycoherent RWTexture2DArray<float> rw_tex2d_array_glc;
globallycoherent RWTexture3D<float> rw_tex3d_glc;

groupshared uint gt = 0;
groupshared struct {
	float foo;
	uint bar;
} gstruct[3];

[numthreads(4, 2, 1)]
void main()
{
	uint tally = 0; // Used to thwart compiler optimisations
	uint ret;

	// RWByteAddressBuffer operations:
	rw_byte_buf.GetDimensions(ret);
	tally += ret;

	// imm_atomic_iadd
	rw_byte_buf.InterlockedAdd(0, 1, ret);
	tally += ret;

	// atomic_iadd:
	rw_byte_buf.InterlockedAdd(0, 1);

	// imm_atomic_and
	rw_byte_buf.InterlockedAnd(0, true, ret);
	tally += ret;

	// atomic_and
	rw_byte_buf.InterlockedAnd(0, false);

	// imm_atomic_cmp_exch
	rw_byte_buf.InterlockedCompareExchange(0, 1, 6, ret);
	tally += ret;

	// atomic_cmp_store
	rw_byte_buf.InterlockedCompareStore(0, 2, 3);

	// imm_atomic_exch
	rw_byte_buf.InterlockedExchange(0, 5, ret);
	tally += ret;

	// imm_atomic_imax
	rw_byte_buf.InterlockedMax(0, 6, ret);
	tally += ret;

	// atomic_imax
	rw_byte_buf.InterlockedMax(0, 7);

	// imm_atomic_umax
	rw_byte_buf.InterlockedMax(0, uint(6), ret);
	tally += ret;

	// atomic_umax
	rw_byte_buf.InterlockedMax(0, uint(7));

	// imm_atomic_imin
	rw_byte_buf.InterlockedMin(0, 4, ret);
	tally += ret;

	// atomic_imin
	rw_byte_buf.InterlockedMin(0, 2);

	// imm_atomic_umin
	rw_byte_buf.InterlockedMin(0, uint(4), ret);
	tally += ret;

	// atomic_umin
	rw_byte_buf.InterlockedMin(0, uint(2));

	// imm_atomic_or
	rw_byte_buf.InterlockedOr(0, 7, ret);
	tally += ret;

	// atomic_or
	rw_byte_buf.InterlockedOr(0, 9);

	// imm_atomic_xor
	rw_byte_buf.InterlockedXor(0, 8, ret);
	tally += ret;

	// atomic_xor
	rw_byte_buf.InterlockedXor(0, 9);

	// ld_raw_indexable
	tally += rw_byte_buf.Load(1);

	// store_raw
	rw_byte_buf.Store(5, 8);


	// Access globally coherent UAVs:
	tally += rw_byte_buf_glc.Load(0);
	tally += rw_structured_glc[0].foo;
	tally += rw_buf_glc[0];
	tally += rw_tex1d_glc[0];
	tally += rw_tex1d_array_glc[uint2(0, 0)];
	tally += rw_tex2d_glc[uint2(0, 0)];
	tally += rw_tex2d_array_glc[uint3(0, 0, 0)];
	tally += rw_tex3d_glc[uint3(0, 0, 0)];


	// Misc functions

	// firstbit_hi
	tally += firstbithigh(tally);

	// firstbit_lo
	tally += firstbitlow(tally);

	// firstbit_shi
	tally += firstbithigh(asint(tally));

	// msad
	tally += msad4(tally, uint2(1, 2), uint4(3, 4, 5, 6));




	// Various synchronisation types:


	// sync_uglobal_g
	AllMemoryBarrier();

	InterlockedAdd(gt, 1, ret);
	tally += ret;

	// sync_uglobal_g_t
	AllMemoryBarrierWithGroupSync();

	InterlockedAdd(gt, 1, ret);
	tally += ret;

	// sync_uglobal
	DeviceMemoryBarrier();

	InterlockedAdd(gt, 1, ret);
	tally += ret;

	// sync_uglobal_t
	DeviceMemoryBarrierWithGroupSync();

	InterlockedAdd(gt, 1, ret);
	tally += ret;

	// sync_g
	GroupMemoryBarrier();

	InterlockedAdd(gt, 1, ret);
	tally += ret;

	// sync_g_t
	GroupMemoryBarrierWithGroupSync();

	InterlockedAdd(gt, 1, ret);
	tally += ret;

	// TODO: sync_ugroup*
	// TODO: sync_ugroup_t*
	// TODO: sync_ugroup_g*
	// TODO: sync_ugroup_g_t*

	tally += gstruct[2].bar;

	// Use the tally so the compiler doesn't optimise anything out:
	rw_byte_buf.InterlockedAdd(1, tally);
}
