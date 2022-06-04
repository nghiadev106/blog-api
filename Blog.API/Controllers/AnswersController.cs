﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.API.Helpers;
using Blog.Application.Interfaces;
using Blog.Shared.Answers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IMapper _mapper;

        public AnswersController(
            IAnswerService answerService,
            IMapper mapper)
        {
            _answerService = answerService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> GetAll()
        {
            var answers = await _answerService.GetAll();
            var answersVm = _mapper.Map<IEnumerable<AnswerDto>>(answers);
            return Ok(answersVm);
        }

        [HttpGet("question/{questionId}")]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> GetAnswerByQuestionId(int questionId)
        {
            var answers = await _answerService.GetAnswerByQuestionId(questionId);
            var answersVm = _mapper.Map<IEnumerable<AnswerDto>>(answers);
            return Ok(answersVm);
        }

        [HttpGet("{answerId}")]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> GetById(int answerId)
        {
            var answer = await _answerService.GetById(answerId);
            if (answer == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found answer with id {answerId}"));
            var answerVm = _mapper.Map<AnswerDto>(answer);
            return Ok(answerVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnswerCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse("Answer id not valid"));
            }
            var result = await _answerService.Add(request);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Update answer failed"));
            }
        }

        [HttpPut("{answerId}")]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> Update([FromRoute] int answerId, [FromBody] AnswerUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Answer id not valid"));
            }

            var answer = await _answerService.GetById(answerId);
            if (answer == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found answer with id {answerId}"));

            answer.QuestionId = request.QuestionId;
            answer.Content = request.Content;

            var result = await _answerService.Update(answer);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Update answer failed"));
            }
        }

        [HttpDelete("{answerId}")]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var answer = await _answerService.GetById(answerId);
            if (answer == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found answer with id {answerId}"));

            var result = await _answerService.Delete(answer);

            if (result > 0)
            {
                return Ok(answer);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Delete answer failed"));
            }
        }
    }
}
