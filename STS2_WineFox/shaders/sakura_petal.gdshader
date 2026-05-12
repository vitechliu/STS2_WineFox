shader_type canvas_item;

uniform sampler2D atlas_texture : source_color;
uniform int atlas_columns = 4;
uniform int atlas_rows = 4;
uniform float rotation_speed_min = 0.5;
uniform float rotation_speed_max = 2.0;
uniform float wobble_amplitude = 0.3;
uniform float wobble_speed = 1.5;

// 每个粒子的随机种子，由粒子系统传入 INSTANCE_CUSTOM.x
varying float v_random_seed;
varying vec2 v_particle_uv;

void vertex() {
    v_random_seed = INSTANCE_CUSTOM.x;
    v_particle_uv = UV;
}

void fragment() {
    // 计算当前粒子使用图集中的哪一格
    int total_frames = atlas_columns * atlas_rows;
    int frame_index = int(mod(v_random_seed * 100.0, float(total_frames)));
    int col = frame_index % atlas_columns;
    int row = frame_index / atlas_columns;

    float col_f = float(col);
    float row_f = float(row);
    float atlas_cols_f = float(atlas_columns);
    float atlas_rows_f = float(atlas_rows);

    // 基础 UV（图集内单格）
    vec2 atlas_uv = vec2(
        (v_particle_uv.x + col_f) / atlas_cols_f,
        (v_particle_uv.y + row_f) / atlas_rows_f
    );

    // 伪 3D 旋转：基于时间和随机种子计算旋转角度
    float time = TIME;
    float rot_speed = mix(rotation_speed_min, rotation_speed_max, fract(v_random_seed * 17.31));
    float angle = time * rot_speed + v_random_seed * 6.283185;

    // 摇摆效果（模拟飘落时的左右摆动）
    float wobble = sin(time * wobble_speed + v_random_seed * 23.71) * wobble_amplitude;
    angle += wobble;

    // 2D 旋转矩阵
    float cos_a = cos(angle);
    float sin_a = sin(angle);
    mat2 rot = mat2(
        vec2(cos_a, -sin_a),
        vec2(sin_a, cos_a)
    );

    // 以中心为原点旋转 UV
    vec2 centered_uv = v_particle_uv - vec2(0.5);
    vec2 rotated_uv = rot * centered_uv;
    vec2 final_uv = rotated_uv + vec2(0.5);

    // 重新映射到图集坐标
    vec2 final_atlas_uv = vec2(
        (final_uv.x + col_f) / atlas_cols_f,
        (final_uv.y + row_f) / atlas_rows_f
    );

    // 边界检查，超出单格范围则丢弃（避免采样到相邻花瓣）
    if (final_uv.x < 0.0 || final_uv.x > 1.0 || final_uv.y < 0.0 || final_uv.y > 1.0) {
        discard;
    }

    vec4 col_tex = texture(atlas_texture, final_atlas_uv);
    COLOR = col_tex;
}
