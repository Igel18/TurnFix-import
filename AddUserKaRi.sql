-- Create User KaRi1 and set privileges 

CREATE USER "KaRi1" WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_bereiche_int_bereicheid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_disgrp_x_disziplinen_int_disgrp_x_disziplinenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_disziplinen_felder_int_disziplinen_felderid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_disziplinen_gruppen_int_disziplinen_gruppenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_disziplinen_int_disziplinenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_formeln_int_formelid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_gaue_int_gaueid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_gruppen_int_gruppenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_gruppen_x_teilnehmer_int_gruppen_x_teilnehmerid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_jury_results_int_juryresultsid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_konten_int_kontenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_laender_int_laenderid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_layout_felder_int_layout_felderid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_layouts_int_layoutid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_man_x_man_ab_int_man_x_man_abid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_man_x_teilnehmer_int_man_x_teilnehmerid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_mannschaften_abzug_int_mannschaften_abzugid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_mannschaften_int_mannschaftenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_personen_int_personenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_quali_leistungen_int_quali_leistungenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_riegen_x_disziplinen_int_riegen_x_disziplinenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_sport_int_sportid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_startreihenfolge_int_startreihenfolgeid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_status_int_statusid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_teilnehmer_int_teilnehmerid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_veranstaltungen_int_veranstaltungenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_verbaende_int_verbaendeid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_vereine_int_vereineid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wertungen_details_int_wertungen_detailsid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wertungen_int_wertungenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wertungen_x_disziplinen_int_wertungen_x_disziplinenid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wettkaempfe_dispos_int_wettkaempfe_disposid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wettkaempfe_int_wettkaempfeid_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wettkaempfe_x_disziplinen_int_wettkaempfe_x_disziplinen_seq TO "KaRi1";

GRANT SELECT, USAGE ON SEQUENCE turnfix.public.tfx_wettkampforte_int_wettkampforteid_seq TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_bereiche TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_disgrp_x_disziplinen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_disziplinen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_disziplinen_felder TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_disziplinen_gruppen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_formeln TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_gaue TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_gruppen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_gruppen_x_teilnehmer TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_jury_results TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_konten TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_laender TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_layout_felder TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_layouts TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_man_x_man_ab TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_man_x_teilnehmer TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_mannschaften TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_mannschaften_abzug TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_personen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_quali_leistungen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_riegen_x_disziplinen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_sport TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_startreihenfolge TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_status TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_teilnehmer TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_veranstaltungen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_verbaende TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_vereine TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wertungen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wertungen_details TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wertungen_x_disziplinen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wettkaempfe TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wettkaempfe_dispos TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wettkaempfe_x_disziplinen TO "KaRi1";

GRANT SELECT ON TABLE turnfix.public.tfx_wettkampforte TO "KaRi1";


GRANT ALL ON TABLE turnfix.public.tfx_wertungen_details TO "KaRi1";

GRANT ALL ON TABLE turnfix.public.tfx_wertungen_x_disziplinen TO "KaRi1";

GRANT ALL ON TABLE turnfix.public.tfx_status TO "KaRi1";

