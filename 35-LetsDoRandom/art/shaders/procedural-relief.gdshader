shader_type spatial;

uniform sampler2D color_tex : hint_default_black;
uniform sampler2D relief_tex : hint_default_black;
uniform float amplitude = 1.0;

void vertex() {
	float height = texture(relief_tex, UV).r;
	VERTEX.y += height * amplitude;
	COLOR.rgb = vec3(height);
}

void fragment() {
	ALBEDO = texture(color_tex, vec2(COLOR.r, 0.0)).rgb;
}
