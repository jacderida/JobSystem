using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Instruments;

namespace JobSystem.BusinessLogic.Services
{
    public class InstrumentService : ServiceBase
    {
        private readonly IInstrumentRepository _instrumentRepository;

        public InstrumentService(
            IUserContext applicationContext,
            IInstrumentRepository instrumentRepository,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _instrumentRepository = instrumentRepository;
        }

        public Instrument Create(Guid id, string manufacturer, string modelNo, string range, string description, int allocatedCalibrationTime)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the instrument");
            var instrument = new Instrument();
            instrument.Id = id;
            instrument.Manufacturer = manufacturer;
            instrument.ModelNo = modelNo;
            instrument.Range = range;
            instrument.Description = description;
            instrument.AllocatedCalibrationTime = GetAllocatedCalibrationTime(allocatedCalibrationTime);
            ValidateAnnotatedObjectThrowOnFailure(instrument);
            _instrumentRepository.Create(instrument);
            return instrument;
        }

        public Instrument Edit(Guid id, string manufacturer, string modelNo, string range, string description, int allocatedCalibrationTime)
        {
            var instrument = _instrumentRepository.GetById(id);
            if (instrument == null)
                throw new ArgumentException("An ID must be supplied for the instrument");
            instrument.Manufacturer = manufacturer;
            instrument.ModelNo = modelNo;
            instrument.Range = range;
            instrument.Description = description;
            instrument.AllocatedCalibrationTime = GetAllocatedCalibrationTime(allocatedCalibrationTime);
            ValidateAnnotatedObjectThrowOnFailure(instrument);
            _instrumentRepository.Update(instrument);
            return instrument;
        }

        public Instrument GetById(Guid id)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            var instrument = _instrumentRepository.GetById(id);
            if (instrument == null)
                throw new ArgumentException("A valid ID must be supplied for the instrument ID");
            return instrument;
        }

        public int GetInstrumentsCount()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _instrumentRepository.GetInstrumentsCount();
        }

        public IEnumerable<Instrument> GetInstruments()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _instrumentRepository.GetInstruments();
        }

        public IEnumerable<Instrument> SearchByKeyword(string keyword)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _instrumentRepository.SearchByKeyword(keyword);
        }

        public IEnumerable<string> SearchManufacturerByKeyword(string keyword)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _instrumentRepository.SearchManufacturerByKeyword(keyword);
        }

        public IEnumerable<string> SearchModelNoByKeywordFilterByManufacturer(string keyword, string manufacturer)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _instrumentRepository.SearchModelNoByKeywordFilterByManufacturer(keyword, manufacturer);
        }

        private int GetAllocatedCalibrationTime(int allocatedCalibrationTime)
        {
            if (allocatedCalibrationTime < 15 || allocatedCalibrationTime > 1000)
                throw new DomainValidationException(Messages.InvalidAllocatedCalibrationTime, "AllocatedCalibrationTime");
            return allocatedCalibrationTime;
        }
    }
}