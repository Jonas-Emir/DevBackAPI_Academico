﻿using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using GestaoEstoque_API.Application.Dtos;

namespace GestaoEstoque_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;

        public FornecedorController(IFornecedorRepositorio fornecedorRepositorio)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        [HttpGet("ListarFornecedores")]
        public ActionResult<List<FornecedorResponseDto>> ListarFornecedores()
        {
            try
            {
                var fornecedores = _fornecedorRepositorio.BuscarFornecedores();
                return Ok(fornecedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar fornecedores: {ex.Message}");
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public ActionResult<FornecedorResponseDto> BuscarPorId(int id)
        {
            try
            {
                var fornecedor = _fornecedorRepositorio.BuscarPorId(id);

                if (fornecedor == null)
                    return NotFound($"Fornecedor com ID {id} não encontrado.");

                return Ok(fornecedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar fornecedor: {ex.Message}");
            }
        }

        [HttpPost("InserirFornecedor")]
        public async Task<ActionResult<FornecedorRequestDto>> Adicionar([FromBody] FornecedorRequestDto fornecedor)
        {
            try
            {
                var fornecedorCadastrado = await _fornecedorRepositorio.AdicionarAsync(fornecedor);
                return Ok(fornecedorCadastrado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar fornecedor: {ex.Message}");
            }
        }

        [HttpPut("AtualizarFornecedor/{id}")]
        public async Task<ActionResult<FornecedorRequestDto>> Atualizar([FromBody] FornecedorRequestDto fornecedorDto, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var fornecedorExistente = _fornecedorRepositorio.BuscarPorId(id);
                if (fornecedorExistente == null)
                    return NotFound($"Fornecedor com ID {id} não encontrado.");

                var fornecedorAtualizado = await _fornecedorRepositorio.AtualizarAsync(fornecedorDto, id);
                return Ok(fornecedorAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar fornecedor: {ex.Message}");
            }
        }

        [HttpDelete("ApagarFornecedor/{id}")]
        public ActionResult<bool> ApagarFornecedor(int id)
        {
            try
            {
                var resposta = _fornecedorRepositorio.Apagar(id);
                if (string.IsNullOrEmpty(resposta))
                    return NotFound($"Fornecedor com ID {id} não encontrado.");

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar fornecedor: {ex.Message}");
            }
        }
    }
}
