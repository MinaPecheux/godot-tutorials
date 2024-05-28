shader_type canvas_item;

uniform vec3 color : source_color = vec3(1.0);

void fragment() {
	vec3 col = texture(TEXTURE, UV).rgb;
	float mask = smoothstep(0.15, 1.0, col.b);
	COLOR.rgb = mask * color + (1.0 - mask) * col;
}
