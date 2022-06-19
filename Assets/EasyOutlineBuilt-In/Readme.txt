Ouline For Built-In 

Thanks for your purchase. I hope you will find the asset useful.
If you have any problem, please email me: izzynab.publisher@gmail.com

You can use outline shader by adding OnlyOutline material as a second material to your object. If your object has more than one submeshes, 
it won't work, and you have to use standard lighing shaders.
Two outline shaders that work just like unity default shaders are: StandardOutlineMetallic and StandardOutlineSpecular.


--------------------------------------------------------
Properties:
--------------------------------------------------------

Use Adaptive thickness to make outline thickness more steady over distance from mesh. 

There are 3 outline types: 
Normal - based on mesh normals
Position - based on mesh vertex positions
UV Baked - reads values baked in UV3 channel, if you need to bake to another channel,in BakeMeshNormalsToUV tool choose UV2 or UV 1
and change UV node in shader graph to read from appropriate uv channel.

--------------------------------------------------------
Tools:
--------------------------------------------------------

To bake mesh smoothed normals to UV channel, use Tools/BakeMeshNormalsToUV window. Please, make sure you selected UV3 channel for baking normals.
If you are not able to use UV3 channel, you have to change shader code by yourself to make it work with another uv channel. 
To do that, simply find this lines of code in the shader:

#elif defined(_OUTLINETYPE_UVBAKED)
				float3 staticSwitch14 = float3( v.texcoord3.xy ,  0.0 );

And change texcoord3 to the UV you choosed. Example:
If you baked normals to UV2 channel, change texcoord3 to texcoord2.

