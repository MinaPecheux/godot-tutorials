shader_type canvas_item;

uniform vec3 tint : source_color = vec3(1.0, 1.0, 1.0);
uniform float threshold : hint_range(0.0, 1.0) = 0.5;
uniform float smoothing : hint_range(0.0, 1.0) = 0.5;

void fragment() {
	float s = (1.0 - smoothing) / 2.0;
	float mask = smoothstep(s, 1.0 - s, 0.5 + UV.y - threshold);
	COLOR.rgb = tint;
	COLOR.a = mask;//mix(atmo * tint, col, mask);
}
