Shader "Selfmade/Visible"
{
	Properties
	{
		_Color("Main Color", Color) = (0,0,1,0.7)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Pass
	{
		Material
	{
		Diffuse[_Color]
	}
		Lighting On
		ZTest Always
		ZWrite On
	}
	}
}
