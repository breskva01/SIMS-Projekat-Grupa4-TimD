using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class MyRenovationAppointmentsViewModel : ViewModelBase
    {
        private AccommodationRenovationService _renovationService;
        public ObservableCollection<AccommodationRenovation> Appointments { get; set; }
        public AccommodationRenovation SelectedAppointment { get; set; }
        public ICommand CancelAppointmentCommand { get; }
        public MyRenovationAppointmentsViewModel(int id) 
        {
            _renovationService = new AccommodationRenovationService();
            Appointments = new ObservableCollection<AccommodationRenovation>(_renovationService.GetAllAppointmentsByOwner(id));
            CancelAppointmentCommand = new ExecuteMethodCommand(CancelAppointment);
        }
        private void CancelAppointment()
        {
            if (SelectedAppointment == null)
            {
                MessageBox.Show("You have to select an appointment!");
            }
            else
            {
                _renovationService.CancelAppointment(SelectedAppointment.Id);
                var appointments = _renovationService.GetAllAppointmentsByOwner(SelectedAppointment.Id);
                Appointments.Clear();
                foreach (var appointment in appointments)
                {
                    Appointments.Add(appointment);
                }
            }
        }
    }
}
