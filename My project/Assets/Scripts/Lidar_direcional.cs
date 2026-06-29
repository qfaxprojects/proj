using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class Lidar_direcional : MonoBehaviour
{
    [SerializeField] private InputActionAsset mapa;

    private InputAction _fire;
    private InputAction _changeRadius;
    private List<Vector3> _positionsList = new();
    private List<int> _enemyPositions = new();
    private List<VisualEffect> _vfxList = new();
    private VisualEffect _currentVFX;
    private Texture2D _texture;
    private Color[] _positions;
    private bool _createNewVFX;
    private LineRenderer _lineRenderer;
    [SerializeField]
    [Tooltip("Supostamente aumenta o número de pontos de acordo com a normal desse.\n" +
        "Tem uma função que aumenta a densidade enquanto se segura o botao de acao.")]
    private float density = 0;

    private const string TEXTURE_NAME = "PositionsTexture";
    private const string RESOLUTION_PARAMETER_NAME = "Resolution";


    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private VisualEffect _vfxPrefab;
    [SerializeField] private GameObject _vfxContainer;
    [SerializeField] private int _maxContainer = 5;
    [SerializeField] private int _indiceContainer = 0;

    [SerializeField] private Transform _castPoint;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _maxRadius = 10f;
    [SerializeField] private float _minRadius = 1f;
    
    [SerializeField] private float _range = 10f;

    [SerializeField] private int resolution = 100;
    [SerializeField][Tooltip("Quanto maior, mais objs cria(e mais rapido)")] private int _pointsPerScan = 100;
    public TextMeshProUGUI texto;


    private void OnEnable()
    {
        mapa.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        mapa.FindActionMap("Player").Disable();
    }


    private void Awake()
    {
        _fire = InputSystem.actions.FindAction("Lidar");
        _changeRadius = InputSystem.actions.FindAction("Scroll");
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _createNewVFX = true;


        CreateNewVFX();
        ApplyPositions();
    }

    private void FixedUpdate()
    {
        Scan();
        //ChangeRadius();
    }

    private void ChangeRadius()
    {
        if (_changeRadius.triggered)
        {
            _radius = Math.Clamp(_radius + _changeRadius.ReadValue<float>(), _minRadius, _maxRadius);
        }
    }

    private void Scan()
    {
        /* adaptação ponto a ponto(vvai precisar de vvvarios raycasts)
        int i = 0;

        if (_fire.IsPressed())
        {
            if (i < _pointsPerScan)
            {
                i++;
                _lineRenderer.enabled = true;

                Vector3 randomPoint = Random.insideUnitSphere;
                randomPoint.z = Math.Abs(randomPoint.z);

                randomPoint += _castPoint.position + _castPoint.TransformDirection(randomPoint) * _radius;



                Vector3 direction = (randomPoint - transform.position).normalized;

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _range, _layerMask))
                {
                    if (_positionsList.Count < resolution * resolution - 1)
                    {
                        _positionsList.Add(hit.point);
                        _lineRenderer.enabled = true;
                        _lineRenderer.SetPositions(new[]
                        {
                                transform.position,
                                hit.point
                            });
                        _particleAmount++;
                    }
                    else
                    {
                        _createNewVFX = true;
                        CreateNewVFX();
                    }
                }
                ApplyPositions();
            }
            else
            {
                i = 0;
            }
        }
        
        
    
        else 
        {
            _lineRenderer.enabled = false;
        }
        */
        
        if (_fire.IsPressed())
        {
            /*
            density += Time.deltaTime * 25;*/
            texto.text = $"density = {density}\npoints_scan = {_pointsPerScan}\n resolution = {resolution * resolution}";

            //release_points(1);
            solta_novo();
        }
        else
        {
            if (density > 0.001f)
            {
                if (density > 100) { release_points(100); }
                else { release_points(Math.Max(1, density)); }
                
            }
            _lineRenderer.enabled = false;
            density = 0;
        
        }
        

    }

    private void release_points(float density)
    {
        for (int i = 0; i < _pointsPerScan; i++)
        {
            Vector3 randomPoint = Random.insideUnitSphere;

            //if (randomPoint.x < 0.3f && randomPoint.x > -0.3f) { randomPoint.x *= 3*Random.Range(1.2f, 3); }
            //if (randomPoint.z < 0.3f && randomPoint.z > -0.3f) { randomPoint.z *= 3*Random.Range(1.2f, 3); }

            randomPoint.z = Math.Abs(randomPoint.z);

           // float inter = Mathf.Lerp(0.5f, 7f, Random.Range(0.01f, 0.5f));
            //Debug.Log("[qfaxas] lerp = " + inter);
            randomPoint += _castPoint.position + _castPoint.TransformDirection(randomPoint) * _radius;// _radius;



            Vector3 direction = (randomPoint - transform.position).normalized;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _radius, _layerMask)) //antes do inter era _range, se der merda muda
            {
                Debug.Log(_positionsList.Count);
                if (_positionsList.Count < resolution * resolution)
                {
                    _positionsList.Add(hit.point);
                    //_lineRenderer.enabled = true;

                    //checa se é um inimigo e adiciona o indice para mudar dps
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        _enemyPositions.Add(_positionsList.Count-1);
                        Debug.Log("[qfaxas] Inimigo em " + i);
                    }


                    /*_lineRenderer.SetPositions(new[]
                    {
                            transform.position,
                            hit.point
                        });*/
                }
                else
                {
                    Debug.Log("chama");
                    _createNewVFX = true;
                    CreateNewVFX();
                    release_points(density);
                    break;
                }
            }
        }
        
        ApplyPositions();
    }



    private void CreateNewVFX()
    {
        if (!_createNewVFX) return;

        if (_vfxList.Count < _maxContainer)
        {
            _vfxList.Add(_currentVFX);
        }
        else
        {

            _vfxList.RemoveAt(0);
            _vfxList.Add(_currentVFX);
            VisualEffect temp = _vfxContainer.gameObject.GetComponentInChildren<VisualEffect>();
            Debug.Log(temp.gameObject);
            Destroy(temp.gameObject);


        }


        _currentVFX = Instantiate(_vfxPrefab, transform.position, Quaternion.identity, _vfxContainer.transform);
        _currentVFX.SetUInt(RESOLUTION_PARAMETER_NAME, (uint)resolution);

        _texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);

        _positions = new Color[resolution * resolution];

        _positionsList.Clear();
        _enemyPositions.Clear();

        _createNewVFX = false;
    }

    private void solta_novo()
    {
        for (int i = 0; i < _pointsPerScan; i++)
        {
            Vector3 randomPoint = Random.insideUnitSphere;

            //if (randomPoint.x < 0.3f && randomPoint.x > -0.3f) { randomPoint.x *= 3*Random.Range(1.2f, 3); }
            //if (randomPoint.z < 0.3f && randomPoint.z > -0.3f) { randomPoint.z *= 3*Random.Range(1.2f, 3); }

            randomPoint.z = Math.Abs(randomPoint.z);

            // float inter = Mathf.Lerp(0.5f, 7f, Random.Range(0.01f, 0.5f));
            //Debug.Log("[qfaxas] lerp = " + inter);
            randomPoint += _castPoint.position + _castPoint.TransformDirection(randomPoint) * _radius;// _radius;



            Vector3 direction = (randomPoint - transform.position).normalized;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _radius, _layerMask)) //antes do inter era _range, se der merda muda
            {
                Debug.Log(_positionsList.Count);
                if (_positionsList.Count < resolution * resolution)
                {
                    _positionsList.Add(hit.point);
                    //_lineRenderer.enabled = true;

                    //checa se é um inimigo e adiciona o indice para mudar dps
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        _enemyPositions.Add(_positionsList.Count - 1);
                        Debug.Log("[qfaxas] Inimigo em " + i);
                    }


                    /*_lineRenderer.SetPositions(new[]
                    {
                            transform.position,
                            hit.point
                        });*/
                }
                else
                {
                    Debug.Log("chama");
                    _createNewVFX = true;
                    cria_novo();
                    break;
                }
            }
        }

        ApplyPositions();
    }
    private void cria_novo()
    {
        if (!_createNewVFX) return;

        VisualEffect atual;

        if (_indiceContainer < _maxContainer-1)
        {
            atual = _vfxContainer.GetComponentsInChildren<VisualEffect>()[_indiceContainer];
        }
        else
        {

            _indiceContainer = 0;
            atual = _vfxContainer.GetComponentsInChildren<VisualEffect>()[0];
            Texture2D textura = atual.GetTexture();
            
            VisualEffect temp = _vfxContainer.gameObject.GetComponentInChildren<VisualEffect>();
            Debug.Log(temp.gameObject);
            Destroy(temp.gameObject);


        }


        _currentVFX = Instantiate(_vfxPrefab, transform.position, Quaternion.identity, _vfxContainer.transform);
        _currentVFX.SetUInt(RESOLUTION_PARAMETER_NAME, (uint)resolution);

        _texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false);

        _positions = new Color[resolution * resolution];

        _positionsList.Clear();
        _enemyPositions.Clear();

        _createNewVFX = false;
    }

    private void ApplyPositions()
    {
        Vector3[] pos = _positionsList.ToArray();

        Vector3 vfxPos = _currentVFX.transform.position;

        Vector3 transformPos = transform.position;

        int loopLength = _texture.width * _texture.height;
        int posListLen = pos.Length;

        for (int i = 0; i < loopLength; i++)
        {
            Color data;
            if (i < posListLen - 1)
            {
                
                data = new Color(pos[i].x - vfxPos.x, pos[i].y - vfxPos.y, pos[i].z - vfxPos.z, 1);

            }
            else
            {
                data = new Color(0,0,0);
            }

            _positions[i] = data;
        }

        foreach (int e in _enemyPositions)
        {
            _positions[e].a = 0.5f;
        }

        _texture.SetPixels(_positions);
        _texture.Apply();

        _currentVFX.SetTexture(TEXTURE_NAME, _texture);
        _currentVFX.Reinit();
    }
}
