shader_type canvas_item;

uniform vec3 light_color : source_color;
uniform vec3 dark_color : source_color;
uniform float emission = 1.0;
uniform float displacement_strength = 0.01;
uniform float displacement_speed = 5.0;
uniform sampler2D noise_tex : repeat_enable;
uniform sampler2D noise_tex2 : repeat_enable;

float soft_light(float base, float blend){
	float limit = step(0.5, blend);
	return mix(2.0 * base * blend + base * base * (1.0 - 2.0 * blend), sqrt(base) * (2.0 * blend - 1.0) + (2.0 * base) * (1.0 - blend), limit);
}

void fragment() {
	// read + mix procedural math noises
	float noise = texture(noise_tex, vec2(
		UV.x, UV.y + TIME * 0.2
	)).r;
	float noise2 = texture(noise_tex2, vec2(
		UV.x, UV.y + TIME * 0.05
	)).r;
	float flames = soft_light(noise, noise2);
	flames = soft_light(smoothstep(0.15, 0.9, UV.y), flames);
	
	// compute color mix based on noises + vertical gradient
	vec3 col = mix(dark_color, light_color, flames);
	col = mix(col, vec3(1.0), smoothstep(0.7, 1.1, UV.y));
	
	// compute alpha mask based on teardrop mask texture
	// + procedural flame noises + vertical gradient
	vec2 mask_uv = UV;
	mask_uv.x += (1.0 - UV.y) * displacement_strength * cos(VERTEX.y * 0.03 + TIME * displacement_speed);
	float teardrop_mask = texture(TEXTURE, mask_uv).r;
	float mask = teardrop_mask * smoothstep(0.15, 0.5, UV.y);
	mask = soft_light(mask, flames);
	
	// output final color + alpha values to render
	COLOR.rgb = col * emission;
	COLOR.a = mask;
}
