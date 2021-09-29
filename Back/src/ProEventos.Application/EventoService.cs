using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist geralPersist;
        private readonly IMapper mapper;
        private readonly IEventoPersist eventoPersist;
        public EventoService(IGeralPersist geralPersist, 
                             IEventoPersist eventoPersist,
                             IMapper mapper)
        {
            this.eventoPersist = eventoPersist;
            this.geralPersist = geralPersist;
            this.mapper = mapper;
        }

        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = this.mapper.Map<Evento>(model);

                this.geralPersist.Add<Evento>(evento);

                if (await this.geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await this.eventoPersist.GetEventoByIdAsync(evento.Id, false);

                    return this.mapper.Map<EventoDto>(eventoRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = this.mapper.Map<Evento>(model);

                evento.Id = eventoId;
                this.geralPersist.Update<Evento>(evento);
                
                if (await this.geralPersist.SaveChangesAsync())
                {
                    return this.mapper.Map<EventoDto>(evento);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await this.eventoPersist.GetEventoByIdAsync(eventoId, false);
                if (evento == null) throw new Exception("Evento para exclusão não encontrado.");
                
                this.geralPersist.Delete(evento);
                return await this.geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await this.eventoPersist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                var resultados = this.mapper.Map<EventoDto[]>(eventos);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await this.eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                var resultados = this.mapper.Map<EventoDto[]>(eventos);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await this.eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;

                var resultado = this.mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}