shader_type spatial;

uniform vec4 color : source_color;
uniform float radius = 1.0;

void fragment() {
	vec2 uv = UV * 2.0 - 1.0;
	float r = uv.x * uv.x + uv.y * uv.y;
	float mask = float(r < radius);
	ALBEDO = color.rgb;
	ALPHA = mask * color.a;
}
