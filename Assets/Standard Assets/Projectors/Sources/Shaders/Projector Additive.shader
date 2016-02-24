Shader "Projector/Additive" { 
   Properties { 
  	  _Color ("Main Color", Color) = (1,1,1,1)
      _ShadowTex ("Cookie", 2D) = "" { TexGen ObjectLinear } 
      _FalloffTex ("FallOff", 2D) = "" { TexGen ObjectLinear } 
   } 
   Subshader { 
      Pass { 
         ZWrite off 
         Fog { Color (1, 1, 1) } 
         ColorMask RGB 
         Blend One One 
         Color [_Color]
         SetTexture [_ShadowTex] { 
         	constantColor [_Color]
            combine texture*constant
            Matrix [_Projector]
         } 
         SetTexture [_FalloffTex] { 
            constantColor (0,0,0,0) 
            combine previous lerp (texture) constant 
            Matrix [_ProjectorClip] 
         } 
      }
   }
}