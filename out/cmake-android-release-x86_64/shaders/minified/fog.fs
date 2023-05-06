
vec4 fog(vec4 color, highp vec3 view) {
highp float d = length(view);
if (d > frameUniforms.fogCutOffDistance) {
return color;
}
highp vec3 density = frameUniforms.fogDensity;
highp float falloff = frameUniforms.fogHeightFalloff;
highp float fogOpticalPathAtOneMeter = density.z;
highp float fh = falloff * view.y;
if (abs(fh) > 0.00125) {
fogOpticalPathAtOneMeter = (density.z - density.x * exp(density.y - fh)) / fh;
}
highp float fogOpticalPath = fogOpticalPathAtOneMeter * max(d - frameUniforms.fogStart, 0.0);
float fogTransmittance = exp(-fogOpticalPath);
float fogOpacity = min(1.0 - fogTransmittance, frameUniforms.fogMaxOpacity);
vec3 fogColor = frameUniforms.fogColor;
if (frameUniforms.fogColorFromIbl > 0.0) {
float lod = frameUniforms.iblRoughnessOneLevel;
fogColor *= textureLod(light_iblSpecular, view, lod).rgb;
}
fogColor *= frameUniforms.iblLuminance * fogOpacity;
if (frameUniforms.fogInscatteringSize > 0.0) {
highp float sunOpticalPath =
fogOpticalPathAtOneMeter * max(d - frameUniforms.fogInscatteringStart, 0.0);
float sunTransmittance = exp(-sunOpticalPath);
vec3 sunColor = frameUniforms.lightColorIntensity.rgb * frameUniforms.lightColorIntensity.w;
float sunAmount = max(dot(-shading_view, frameUniforms.lightDirection), 0.0);
float sunInscattering = pow(sunAmount, frameUniforms.fogInscatteringSize);
fogColor += sunColor * (sunInscattering * (1.0 - sunTransmittance));
}
#if   defined(BLEND_MODE_OPAQUE)
#elif defined(BLEND_MODE_TRANSPARENT)
fogColor *= color.a;
#elif defined(BLEND_MODE_ADD)
fogColor = vec3(0.0);
#elif defined(BLEND_MODE_MASKED)
#elif defined(BLEND_MODE_MULTIPLY)
#elif defined(BLEND_MODE_SCREEN)
#endif
color.rgb = color.rgb * (1.0 - fogOpacity) + fogColor;
return color;
}
