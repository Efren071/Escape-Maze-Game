#version 330
// Material
struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float shininess;
};
uniform Material material;

// Point Light
struct PointLight {
	vec3 position;
	
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

	float constant;
    float linear;
    float quadratic;  
};
#define NR_POINT_LIGHTS 14
uniform PointLight pointLight[NR_POINT_LIGHTS];

// Spot Light
struct SpotLight {
	vec3 position;
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
    float linear;
    float quadratic;  
	
	float cutOff;
	float outerCutOff;
};
#define SPOT_LIGHT_ARRAY 1
uniform SpotLight spotLight[SPOT_LIGHT_ARRAY];

// Taking in from vertex shader
in vec2 vTexCoord;
in vec3 vNormal;
in vec3 vFragPos;
uniform vec3 uViewPosition;

uniform sampler2D s_texture;

out vec4 Color;

// Method Prototypes
vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);	// Point Light
vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);		// Spot Light
 
void main()
{
	// Properties
    vec3 norm = normalize(vNormal);
    vec3 viewDir = normalize(uViewPosition - vFragPos);
	vec3 result;

	// phase Point lights
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalculatePointLight(pointLight[i], norm, vFragPos, viewDir);    

	// phase 3: Spot light
	//for (int i = 0; i < SPOT_LIGHT_ARRAY; i++) 
		//result += CalculateSpotLight(spotLight[i], norm, vFragPos, viewDir);   

    Color = vec4(result, 1.0);
}

// Calculate Point Lighting
vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);

    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    // attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    

    // combine results
    vec3 ambient  = light.ambient  * vec3(texture(s_texture, vTexCoord));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(s_texture, vTexCoord));
    vec3 specular = light.specular * spec * vec3(texture(s_texture, vTexCoord));

    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}

// Calculate Spot Lighting
vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{

	vec3 lightDir = normalize(light.position - fragPos);
	
	// check if light is inside spotlight region
	float theta = dot(lightDir, normalize(-light.direction));
	
	if (theta > light.cutOff) 
	{
		// ambient
		vec3 ambient  = light.ambient  * vec3(texture(s_texture, vTexCoord));

		// diffuse
		float diff = max(dot(normal, lightDir), 0.0);


		// specular
		vec3 reflectDir = reflect(-lightDir, normal);
		float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	    
		float epsilon = (light.cutOff - light.outerCutOff);
		float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

		// attenuation
		float distance    = length(light.position - fragPos);
		float attenuation = 1 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
		
		vec3 diffuse  = light.diffuse  * diff * vec3(texture(s_texture, vTexCoord));
		vec3 specular = light.specular * spec * vec3(texture(s_texture, vTexCoord));

		ambient *= attenuation;
		diffuse *= attenuation * intensity;
		specular *= attenuation * intensity;

		return ambient + diffuse + specular;
	}
	else 
	{
		vec3 ambient  = light.ambient  * vec3(texture(s_texture, vTexCoord));

		float diff = max(dot(normal, lightDir), 0.0);
		vec3 diffuse  = light.diffuse  * diff * vec3(texture(s_texture, vTexCoord));

		vec3 reflectDir = reflect(-lightDir, normal);
		float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
		vec3 specular = light.specular * spec * vec3(texture(s_texture, vTexCoord));

		return ambient + diffuse + specular;
	}
}	