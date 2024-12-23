﻿using GestaoEstoque_API.Application.Dtos;
using GestaoEstoque_API.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestaoEstoque_API.Controllers
{
    [Route("api/estoque/")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueRepositorio _estoqueRepositorio;

        public EstoqueController(IEstoqueRepositorio estoqueRepositorio)
        {
            _estoqueRepositorio = estoqueRepositorio;
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<EstoqueResponseDto>>> BuscarTodosEstoques()
        {
            try
            {
                var estoques = await _estoqueRepositorio.BuscarEstoques();
                return Ok(estoques);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar estoques: {ex.Message}");
            }
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<EstoqueResponseDto>> BuscarPorId(int id)
        {
            try
            {
                var estoque = await _estoqueRepositorio.BuscarPorId(id);

                if (estoque == null)
                    return NotFound($"Estoque com ID {id} não encontrado.");

                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar estoque: {ex.Message}");
            }
        }

        [HttpPost("inserir")]
        public async Task<ActionResult<EstoqueRequestDto>> Cadastrar([FromBody] EstoqueRequestDto estoque)
        {
            try
            {
                var estoqueCadastrado = await _estoqueRepositorio.Adicionar(estoque);
                return Ok(estoqueCadastrado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar estoque: {ex.Message}");
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<ActionResult<EstoqueRequestDto>> Atualizar([FromBody] EstoqueRequestDto estoqueDto, int id)
        {
            try
            {
                var estoqueExistente = await _estoqueRepositorio.BuscarPorId(id);

                if (estoqueExistente == null)
                    return NotFound($"Estoque com ID {id} não encontrado.");

                var estoqueAtualizado = await _estoqueRepositorio.Atualizar(estoqueDto, id);
                return Ok(estoqueAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar estoque: {ex.Message}");
            }
        }

        [HttpDelete("apagar/{id}")]
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            try
            {
                bool apagado = await _estoqueRepositorio.Apagar(id);
                if (!apagado)
                    return NotFound($"Estoque com ID {id} não encontrado.");

                return Ok(apagado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar estoque: {ex.Message}");
            }
        }

        [HttpGet("buscar-quantidade-estoque-produto/{produtoId:int}")]
        public async Task<IActionResult> BuscarQuantidadeEstoquePorProduto(int produtoId)
        {
            try
            {
                var estoque = await _estoqueRepositorio.BuscarQuantidadeEstoquePorProduto(produtoId);

                if (estoque == null)
                    return NotFound(new { mensagem = "Produto não encontrado no estoque." });

                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produto no estoque: {ex.Message}");
            }
        }

        [HttpGet("buscar-quantidade-por-tipo-movimento/{produtoId}")]
        public async Task<ActionResult<QuantidadePorTipoMovimentoResponseDto>> GetQuantidadePorTipoMovimento(int produtoId)
        {
            var produto = await _estoqueRepositorio.VerificarSeProdutoExisteAsync(produtoId);

            if (produto == null)
                return NotFound($"Produto com ID {produtoId} não encontrado.");
            var quantidadePorTipo = await _estoqueRepositorio.BuscarQuantidadePorTipoMovimento(produtoId);

            var response = new QuantidadePorTipoMovimentoResponseDto
            {
                ProdutoId = produtoId,
                ProdutoNome = produto.ProdutoNome,
                QuantidadePorTipoMovimento = quantidadePorTipo
            };
            return Ok(response);
        }

        [HttpGet("buscar-quantidade-estoque-total")]
        public async Task<IActionResult> BuscarQuantidadeTotalDeCadaProduto()
        {
            try
            {
                var quantidadePorProduto = await _estoqueRepositorio.BuscarQuantidadeTotalDeCadaProduto();
                return Ok(quantidadePorProduto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = ex.Message });
            }
        }
    }
}
