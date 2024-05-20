shader_type spatial;

uniform vec3 shallow_color : source_color;
uniform vec3 deep_color : source_color;
uniform float waves_amplitude = 1.0;
uniform sampler2D waves_tex : hint_default_black;
uniform sampler2D caustics_tex : hint_default_black;

void vertex() {
	float height = texture(waves_tex, UV + TIME * 0.1).r;
	VERTEX.y += height * waves_amplitude;
	COLOR.r = height;
}

void fragment() {
	float caustics1 = texture(caustics_tex, UV + TIME * 0.03).r;
	float caustics2 = texture(caustics_tex, vec2(1.0) - UV + TIME * 0.03).r;

	vec3 col = mix(deep_color, shallow_color, smoothstep(0.0, 0.75, COLOR.r));
	col += vec3(pow(caustics1, 4.0)) * shallow_color * 5.0;
	col += vec3(pow(caustics2, 3.0)) * shallow_color * 1.5;
	
	ALBEDO = col;
}
