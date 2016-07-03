// Painterly Post Process
// Copyright 2014 Generic Evil Business Ltd.
// http://www.genericevil.com

#define PAINTERLY_SAMPLE(tex, offset) tex2D(tex, i.uv + offset);
#define PAINTERLY_SAMPLE_ROTATE(tex, offset, rotate) tex2D(tex, i.uv + (mul(offset, rotate)));

sampler2D _MainTex;
#ifdef DEPTHFADE
	uniform sampler2D _CameraDepthTexture;
	half4 _depthOffsets = 0; // x = min, y = max, z = slope
#endif

#ifdef GRADE
	half _PaintIntensity;
#endif

// Offset vectors
#if defined(THREE_SAMPLE) || defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
	half2 _Offset1;
	half2 _Offset2;
	half2 _Offset3;
#endif
#if defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
	half2 _Offset4;
#endif
#if defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
	half2 _Offset5;
#endif
#if defined(SIX_SAMPLE)
	half2 _Offset6;
#endif

#ifdef UROTATE
	half _curveWidth = 20;
#endif

struct v2f 
{
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
	#ifdef DEPTHFADE
		 float4 projPos : TEXCOORD1; 
	#endif
};

v2f PainterlyVert(appdata_img v)
{
	v2f o;
	#ifdef DEPTHFADE
		float4 pos = mul(UNITY_MATRIX_MVP, v.vertex);;
		o.projPos = ComputeScreenPos(pos);
		o.pos = pos;
	#else
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	#endif
	
	o.uv =  v.texcoord.xy;	

	return o;
}

#ifdef RANDOMIZE
	float rand(in float2 uv)
	{
		return frac(sin(dot(uv.xy, float2(12.9898, 78.233)))* 43758.5453);
	}
#endif


half4 PainterlyFragment(v2f i) : COLOR
{
	// Sample the screen texture, so we always have some representation of the colour thats supposed to be here...
	#ifdef DEPTHFADE
		half4 mainColour = tex2D(_MainTex, i.uv);
		half4 colour = mainColour;
	#else
		half4 colour = tex2D(_MainTex, i.uv);
	#endif
	
	// If randomization is enabled, generate a rotation matrix
	#if defined(RANDOMIZE)
		const float r = rand(i.uv);
		float s = sin(r);
		float c = cos(r);
		float2x2 rotation = float2x2(c, -s, s, c);
	#elif defined(UROTATE)
		const float r = i.uv.x * _curveWidth;
		float s = sin(r);
		float c = cos(r);
		float2x2 rotation = float2x2(c, -s, s, c);
	#endif
	
	// Gather samples from nearby pixels.
	#if defined(THREE_SAMPLE) || defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
		#ifdef ROTATE
			half4 stroke1 = PAINTERLY_SAMPLE_ROTATE(_MainTex, _Offset1, rotation);
			half4 stroke2 = PAINTERLY_SAMPLE_ROTATE(_MainTex, _Offset2, rotation);
			half4 stroke3 = PAINTERLY_SAMPLE_ROTATE(_MainTex, _Offset3, rotation);
		#else
			half4 stroke1 = PAINTERLY_SAMPLE(_MainTex, _Offset1);
			half4 stroke2 = PAINTERLY_SAMPLE(_MainTex, _Offset2);
			half4 stroke3 = PAINTERLY_SAMPLE(_MainTex, _Offset3);
		#endif
	#endif
	#if defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
		#ifdef ROTATE
			half4 stroke4 = PAINTERLY_SAMPLE_ROTATE(_MainTex, _Offset4, rotation);
		#else		
			half4 stroke4 = PAINTERLY_SAMPLE(_MainTex, _Offset4);
		#endif
	#endif
	#if defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
		#ifdef ROTATE
			half4 stroke5 = PAINTERLY_SAMPLE_ROTATE(_MainTex, _Offset5, rotation);
		#else
			half4 stroke5 = PAINTERLY_SAMPLE(_MainTex, _Offset5);
		#endif
	#endif
	#if defined(SIX_SAMPLE)
		#ifdef ROTATE
			half4 stroke6 = PAINTERLY_SAMPLE_ROTATE(_MainTex, _Offset6, rotation);
		#else
			half4 stroke6 = PAINTERLY_SAMPLE(_MainTex, _Offset6);
		#endif
	#endif

	// Combine the samples back in to a single colour...
	#ifdef INTENSITY
		// seperate the samples based on intensity ranges and take the range with most samples...

		// First, calculate an intensity for each stroke... Done rather stupidly based on pure NUMBERS.
		#if defined(THREE_SAMPLE) || defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
			float iC = (0.299*colour.r + 0.587*colour.g + 0.114*colour.b); //(colour.r + colour.g + colour.b) / 3;
			float i1 = (0.299*stroke1.r + 0.587*stroke1.g + 0.114*stroke1.b); //(stroke1.r + stroke1.g + stroke1.b) / 3;
			float i2 = (0.299*stroke2.r + 0.587*stroke2.g + 0.114*stroke2.b); //(stroke2.r + stroke2.g + stroke2.b) / 3;
			float i3 = (0.299*stroke3.r + 0.587*stroke3.g + 0.114*stroke3.b); //(stroke3.r + stroke3.g + stroke3.b) / 3;
		#endif
		#if defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
			float i4 = (0.299*stroke4.r + 0.587*stroke4.g + 0.114*stroke4.b); // (stroke4.r + stroke4.g + stroke4.b) / 3;
		#endif
		#if defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
			float i5 = (stroke5.r + stroke5.g + stroke5.b) / 3;
		#endif
		#if defined(SIX_SAMPLE)
			float i6 = (stroke6.r + stroke6.g + stroke6.b) / 3;
		#endif

		// create bucket variables... we support a fixed ammout of buckets right now...
		int i1c = 0;
		int i2c = 0;
		int i3c = 0;
		int i4c = 0;
		int i5c = 0;
		int i6c = 0;
		int i7c = 0;
		int i8c = 0;

		float4 i1v = float4(0,0,0,0);
		float4 i2v = float4(0,0,0,0);
		float4 i3v = float4(0,0,0,0);
		float4 i4v = float4(0,0,0,0);
		float4 i5v = float4(0,0,0,0);
		float4 i6v = float4(0,0,0,0);
		float4 i7v = float4(0,0,0,0);
		float4 i8v = float4(0,0,0,0);

		// put the stroke samples in to buckets....
		#if defined(THREE_SAMPLE) || defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)

			#if defined(SIX_SAMPLE) 
				// some of the six sample shader run out of arithmetic slots! sample colour at a lower precision as a hacky fix for this.
				if (iC <= 0.25) { i2c++; i2v += colour; }
				else if (iC <= 0.5) { i4c++; i4v += colour; }
				else if (iC <= 0.75) { i6c++; i6v += colour; }
				else { i8c++; i8v += colour; }
			#else
				if (iC <= 0.125) { i1c++; i1v += colour; }
				else if (iC <= 0.25) { i2c++; i2v += colour; }
				else if (iC <= 0.375) { i3c++; i3v += colour; }
				else if (iC <= 0.5) { i4c++; i4v += colour; }
				else if (iC <= 0.625) { i5c++; i5v += colour; }
				else if (iC <= 0.75) { i6c++; i6v += colour; }
				else if (iC <= 0.875) { i7c++; i7v += colour; }
				else { i8c++; i8v += stroke1; }
			#endif


			if (i1 <= 0.125) { i1c++; i1v += stroke1; }
			else if (i1 <= 0.25) { i2c++; i2v += stroke1; }
			else if (i1 <= 0.375) { i3c++; i3v += stroke1; }
			else if (i1 <= 0.5) { i4c++; i4v += stroke1; }
			else if (i1 <= 0.625) { i5c++; i5v += stroke1; }
			else if (i1 <= 0.75) { i6c++; i6v += stroke1; }
			else if (i1 <= 0.875) { i7c++; i7v += stroke1; }
			else { i8c++; i8v += stroke1; }

			if (i2 <= 0.125) { i1c++; i1v += stroke2; }
			else if (i2 <= 0.25) { i2c++; i2v += stroke2; }
			else if (i2 <= 0.375) { i3c++; i3v += stroke2; }
			else if (i2 <= 0.5) { i4c++; i4v += stroke2; }
			else if (i2 <= 0.625) { i5c++; i5v += stroke2; }
			else if (i2 <= 0.75) { i6c++; i6v += stroke2; }
			else if (i2 <= 0.875) { i7c++; i7v += stroke2; }
			else { i8c++; i8v += stroke2; }

			if (i3 <= 0.125) { i1c++; i1v += stroke3; }
			else if (i3 <= 0.25) { i2c++; i2v += stroke3; }
			else if (i3 <= 0.375) { i3c++; i3v += stroke3; }
			else if (i3 <= 0.5) { i4c++; i4v += stroke3; }
			else if (i3 <= 0.625) { i5c++; i5v += stroke3; }
			else if (i3 <= 0.75) { i6c++; i6v += stroke3; }
			else if (i3 <= 0.875) { i7c++; i7v += stroke3; }
			else { i8c++; i8v += stroke3; }
		#endif

		#if defined(FOUR_SAMPLE) || defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
			if (i4 <= 0.125) { i1c++; i1v += stroke4; }
			else if (i4 <= 0.25) { i2c++; i2v += stroke4; }
			else if (i4 <= 0.375) { i3c++; i3v += stroke4; }
			else if (i4 <= 0.5) { i4c++; i4v += stroke4; }
			else if (i4 <= 0.625) { i5c++; i5v += stroke4; }
			else if (i4 <= 0.75) { i6c++; i6v += stroke4; }
			else if (i4 <= 0.875) { i7c++; i7v += stroke4; }
			else { i8c++; i8v += stroke4; }
		#endif

		#if defined(FIVE_SAMPLE) || defined(SIX_SAMPLE)
			if (i5 <= 0.125) { i1c++; i1v += stroke5; }
			else if (i5 <= 0.25) { i2c++; i2v += stroke5; }
			else if (i5 <= 0.375) { i3c++; i3v += stroke5; }
			else if (i5 <= 0.5) { i4c++; i4v += stroke5; }
			else if (i5 <= 0.625) { i5c++; i5v += stroke5; }
			else if (i5 <= 0.75) { i6c++; i6v += stroke5; }
			else if (i5 <= 0.875) { i7c++; i7v += stroke5; }
			else { i8c++; i8v += stroke5; }
		#endif

		#if defined(SIX_SAMPLE)
			if (i6 <= 0.125) { i1c++; i1v += stroke6; }
			else if (i6 <= 0.25) { i2c++; i2v += stroke6; }
			else if (i6 <= 0.375) { i3c++; i3v += stroke6; }
			else if (i6 <= 0.5) { i4c++; i4v += stroke6; }
			else if (i6 <= 0.625) { i5c++; i5v += stroke6; }
			else if (i6 <= 0.75) { i6c++; i6v += stroke6; }
			else if (i6 <= 0.875) { i7c++; i7v += stroke6; }
			else { i8c++; i8v += stroke6; }
		#endif

		colour = i1v/i1c;
		if (i2c > i1c)  
		{
			colour = i2v/i2c;
			i1c = i2c;
		}

		if (i3c > i1c)  
		{
			colour = i3v/i3c;
			i1c = i3c;
		}

		if (i4c > i1c)  
		{
			colour = i4v/i4c;
			i1c = i4c;
		}

		if (i5c > i1c)  
		{
			colour = i5v/i5c;
			i1c = i5c;
		}

		if (i6c > i1c)  
		{
			colour = i6v/i6c;
			i1c = i6c;
		}

		if (i7c > i1c)  
		{
			colour = i7v/i7c;
			i1c = i7c;
		}

		if (i8c > i1c)  
		{
			colour = i8v/i8c;
			i1c = i8c;
		}

	#else
		// Nope? Use the much simpler, intrisic based min/max modes
		#if defined(SIX_SAMPLE)
			#if defined(MAX)
				colour = max(colour, max(stroke1, max(stroke2, max(stroke3, max(stroke4, max(stroke5, stroke6))))));
			#elif defined(MIN)
				colour = min(colour, min(stroke1, min(stroke2, min(stroke3, min(stroke4, min(stroke5, stroke6))))));
			#endif
		#elif defined(FIVE_SAMPLE)
			#if defined(MAX)
				colour = max(colour, max(stroke1, max(stroke2, max(stroke3, max(stroke4, stroke5)))));
			#elif defined(MIN)
				colour = min(colour, min(stroke1, min(stroke2, min(stroke3, min(stroke4, stroke5)))));
			#endif
		#elif defined(FOUR_SAMPLE)
			#if defined(MAX)
				colour = max(colour, max(stroke1, max(stroke2, max(stroke3, stroke4))));
			#elif defined(MIN)
				colour = min(colour, min(stroke1, min(stroke2, min(stroke3, stroke4))));
			#endif
		#elif defined(THREE_SAMPLE)
			#if defined(MAX)
				colour = max(colour, max(stroke1, max(stroke2, stroke3)));
			#elif defined(MIN)
				colour = min(colour, min(stroke1, min(stroke2, stroke3)));
			#endif
		#endif
	#endif
	
	// Finally, grade the output colour...
	#ifdef GRADE
		fixed luminance = Luminance(colour.rgb);
		colour.rgb = saturate(lerp(fixed3(luminance, luminance, luminance), colour.rgb, _PaintIntensity));
	#endif

	#ifdef DEPTHFADE
		//Grab the depth value from the depth texture
        //Linear01Depth restricts this value to [0, 1]
		float depth = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
		depth = clamp(depth, _depthOffsets.x, _depthOffsets.y);
		colour = lerp(mainColour, colour, _depthOffsets.z * (depth - _depthOffsets.x));

		//colour.r = depth;
		//colour.g = depth;
		//colour.b = depth;
	#endif

	return colour;
}